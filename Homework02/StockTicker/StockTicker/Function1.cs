using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.Cosmos;

public static class StockTickerFunction
{
    const string EVENT_HUB_CONNECTION_STRING_PLAIN = "Endpoint=sb://stock-ticker.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Q6Xjs7/o1044U2SH/RwKNB4UbcWuzPfe6+AEhAdW6uQ=";

    [FunctionName("GenerateStockDataToEventHub")]
    public static async Task<Microsoft.AspNetCore.Mvc.IActionResult> Run([TimerTrigger("0 */30 * * * *")] TimerInfo _,
        [ServiceBus("StockTickerData", Connection = EVENT_HUB_CONNECTION_STRING_PLAIN)] IAsyncCollector<string> eventHubMessages,
        ILogger log)
    {
        Console.WriteLine($"C# Timer trigger function executed at: {DateTime.Now}");

        List<StockTicker> stockData = GenerateStockData();

        foreach (var data in stockData)
        {
            StockTicker st = new()
            {
                Symbol = data.Symbol,
                Price = data.Price,
                Volume = data.Volume,
                Change = data.Change,
                ChangePercent = data.ChangePercent,
                Date = data.Date
            };

            await eventHubMessages.AddAsync(st.ToString());
            log.LogInformation($"Stock data sent to Event Hub: {st}");
        }

        return new Microsoft.AspNetCore.Mvc.OkResult();
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

    [FunctionName("EventHubTriggerCSharp")]
    public static async Task Run(
        [ServiceBusTrigger("samples-workitems", Connection = EVENT_HUB_CONNECTION_STRING_PLAIN)] EventData message,
        DateTime enqueuedTimeUtc,
        long sequenceNumber,
        string offset,
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
        Microsoft.Azure.Cosmos.Container container = await database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");

        try
        {
            int? throughput = await container.ReadThroughputAsync();
            if (throughput.HasValue)
            {
                log.LogInformation($"Current provisioned throughput : {throughput.Value}");
                int newThroughput = throughput.Value + 100;
                // Update throughput
                await container.ReplaceThroughputAsync(newThroughput);
                log.LogInformation($"New provisioned throughput : : {newThroughput}");
            }
        }
        catch (CosmosException cosmosException) when (cosmosException.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            log.LogError("Cannot read container throuthput.");
            log.LogError($"Exception : {cosmosException.ResponseBody}");
        }

        var dataAsJson = Encoding.UTF8.GetString(message.Body.Array);
        var stockTicker = JsonConvert.DeserializeObject<StockTicker>(dataAsJson);

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
    }
}
