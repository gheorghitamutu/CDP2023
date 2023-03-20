using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Azure.Cosmos;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs.Extensions.WebPubSub;
using Microsoft.Azure.WebPubSub.Common;

public class StockTickerFunction
{
    [FunctionName("GenerateStockDataToEventHub")]
    public void GenerateStockDataToEventHub([TimerTrigger("* * */10 * * *")] TimerInfo timeInfo,
        [ServiceBus("stocktickerdata", Connection = "EVENT_HUB_CONNECTION_STRING_PLAIN")] IAsyncCollector<string> eventHubMessages,
        ILogger log)
    {
        foreach (var data in GenerateStockData())
        {
            eventHubMessages.AddAsync(data.ToString());
            log.LogInformation($"Stock data sent to Event Hub: {data}");
        }
    }

    private static List<StockTicker> GenerateStockData()
    {
        Random rnd = new();
        List<StockTicker> stockData = new();

        List<string> symbols = new() { "AAPL", "AMZN", "GOOGL", "TSLA", "MSFT", "NFLX", "NVDA", "PYPL", "FB", "JPM" };

        foreach (var symbol in symbols)
        {
            stockData.Add(new StockTicker()
            {
                Id = Guid.NewGuid().ToString(),
                PartitionKey = "1",
                Symbol = symbol,
                Price = rnd.Next(100, 1000),
                Volume = rnd.Next(100000, 1000000),
                Change = rnd.Next(-10, 10),
                ChangePercent = rnd.Next(-1000, 1000) / 100.0,
                Date = DateTime.UtcNow
            });
        }

        return stockData;
    }

    public class StockTicker
    {
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }

        [JsonProperty(PropertyName = "partitionKey")]
        public string? PartitionKey { get; set; }

        public string? Symbol { get; set; }

        public double Price { get; set; }

        public int Volume { get; set; }

        public double Change { get; set; }

        public double ChangePercent { get; set; }

        public DateTime Date { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    // // Create a blob container client that the event processor will use 
    // private readonly static BlobContainerClient storageClient = new(BLOB_STORAGE_CONNECTION_STRING, "stockticker");
    // 
    // // Create an event processor client to process events in the event hub
    // private readonly static EventProcessorClient processor = new(
    //     storageClient, EventHubConsumerClient.DefaultConsumerGroupName, EVENT_HUB_CONNECTION_STRING, "stockticker_1679176237014");

    [FunctionName("EventHubTriggerStockTickerData")]
    public async Task EventHubTriggerAdditionToDatabase(
        [EventHubTrigger("stocktickerdata", Connection = "EVENT_HUB_CONNECTION_STRING_PLAIN")] EventData[] events,
        [WebPubSub(Hub = "stocks", Connection = "WEB_PUB_SUB_CONNECTION_STRING_PLAIN")] IAsyncCollector<WebPubSubAction> actions,
        ILogger log)
    {
        // The Azure Cosmos DB endpoint for running this sample.
        string EndpointUri = "https://stockticker.documents.azure.com:443/";

        // The primary key for the Azure Cosmos account.
        string PrimaryKey = "wQD9JhuEn1O7rcE2MoB9sgairaMZ25VvYCMIB8t4KN6dshyqF815sN3lIvVgKt4O5vlfhHZsI3BHACDbmWXMqA==";

        // The name of the database and container we will create
        string databaseId = "StockEntries";
        string containerId = "Items";

        // Create a new instance of the Cosmos Client
        CosmosClient cosmosClient = new(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "StockTickerRealTime" });
        Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        Container container = await database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");

        try
        {
            int? throughput = await container.ReadThroughputAsync();
            if (throughput.HasValue)
            {
                if (throughput.Value <= 900) // max basic account throughput is 1000
                {
                    log.LogInformation($"Current provisioned throughput : {throughput.Value}");
                    int newThroughput = throughput.Value + 100;
                    // Update throughput
                    await container.ReplaceThroughputAsync(newThroughput);
                    log.LogInformation($"New provisioned throughput : : {newThroughput}");
                }
            }
        }
        catch (CosmosException cosmosException) when (cosmosException.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            log.LogError("Cannot read container throuthput.");
            log.LogError($"Exception : {cosmosException.ResponseBody}");
        }

        foreach (EventData data in events)
        {
            var json = Encoding.UTF8.GetString(data.Body.ToArray());
            var stockTicker = JsonConvert.DeserializeObject<StockTicker>(json);

            try
            {
                container = await database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");
                ItemResponse<StockTicker> response = await container.CreateItemAsync(stockTicker);

                log.LogInformation($"Created item in database {stockTicker}.");
            }
            catch (CosmosException ex)
            {
                log.LogInformation($"Failed: {ex.ResponseBody}.");
            }

            try
            {
                await actions.AddAsync(new SendToAllAction
                {
                    Data = BinaryData.FromString(json),
                    DataType = WebPubSubDataType.Json
                });
            }
            catch (Exception ex)
            {
                log.LogInformation($"Failed: {ex.Message}.");
            }
        }

        cosmosClient.Dispose();
    }
}
