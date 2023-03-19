using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Azure.Messaging.EventHubs.Producer;
using System.Text;
using Azure.Messaging.EventHubs;

namespace StockTickerAPI.Controllers
{
    [ApiController]
    [Route("/get_stocks")]
    public class GetStocksController : ControllerBase
    {
        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string? EndpointUri = "https://stockticker.documents.azure.com:443/";

        // The primary key for the Azure Cosmos account.
        private static readonly string? PrimaryKey = "wQD9JhuEn1O7rcE2MoB9sgairaMZ25VvYCMIB8t4KN6dshyqF815sN3lIvVgKt4O5vlfhHZsI3BHACDbmWXMqA==";

        // The Cosmos client instance
        private CosmosClient? cosmosClient;

        // The database we will create
        private Database? database;

        // The container we will create.
        private Container? container;

        // The name of the database and container we will create
        private readonly string databaseId = "StockEntries";
        private readonly string containerId = "Items";

        // // Create a blob container client that the event processor will use 
        // private readonly static BlobContainerClient storageClient = new(BLOB_STORAGE_CONNECTION_STRING, "stockticker");
        // 
        // // Create an event processor client to process events in the event hub
        // private readonly static EventProcessorClient processor = new(
        //     storageClient, EventHubConsumerClient.DefaultConsumerGroupName, EVENT_HUB_CONNECTION_STRING, "stockticker_1679176237014");

        private readonly ILogger<GetStocksController> _logger;

        public GetStocksController(ILogger<GetStocksController> logger) => _logger = logger;

        [HttpGet(Name = "GetStockTicker")]
        public async Task<IEnumerable<StockTicker>> Get()
        {
            {
                // Create a new instance of the Cosmos Client
                cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "StockTickerAPI" });
                database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
                container = await database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");

                try
                {
                    int? throughput = await container.ReadThroughputAsync();
                    if (throughput.HasValue)
                    {
                        _logger.LogInformation($"Current provisioned throughput : {throughput.Value}");
                        int newThroughput = throughput.Value + 100;
                        // Update throughput
                        await container.ReplaceThroughputAsync(newThroughput);
                        _logger.LogInformation($"New provisioned throughput : : {newThroughput}");
                    }
                }
                catch (CosmosException cosmosException) when (cosmosException.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    _logger.LogError("Cannot read container throuthput.");
                    _logger.LogError($"Exception : {cosmosException.ResponseBody}");
                }
            }

            var sqlQueryText = "SELECT * FROM c";
            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new(sqlQueryText);
            FeedIterator<StockTicker>? queryResultSetIterator = container?.GetItemQueryIterator<StockTicker>(queryDefinition);

            List<StockTicker> stocks = new();
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<StockTicker> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (StockTicker stock in currentResultSet)
                {
                    stocks.Add(stock);
                }
            }

            cosmosClient.Dispose();

            return stocks;
        }
    }

    [ApiController]
    [Route("/set_stock")]
    public class SetStockController : ControllerBase
    {
        private readonly static string? EVENT_HUB_CONNECTION_STRING = Environment.GetEnvironmentVariable("EventHub_stockticker_1679176237014");
        private readonly static string? BLOB_STORAGE_CONNECTION_STRING = Environment.GetEnvironmentVariable("BlobStorage_stockticker");
        const string EVENT_HUB_CONNECTION_STRING_PLAIN = "Endpoint=sb://stock-ticker.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Q6Xjs7/o1044U2SH/RwKNB4UbcWuzPfe6+AEhAdW6uQ=";

        private readonly ILogger<SetStockController> _logger;

        public SetStockController(ILogger<SetStockController> logger) => _logger = logger;

        [HttpPost(Name = "SetStock")]
        public async Task<IActionResult> SetStock(string symbol, double price, int volume, double change, double changePercent, DateTime date)
        {
            _logger.LogInformation($"StockTickerFunction HTTP trigger function started {HttpContext.Response}.");

            StockTicker st = new()
            {
                Symbol = symbol,
                Price = price,
                Volume = volume,
                Change = change,
                ChangePercent = changePercent,
                Date = date
            };

            EventHubProducerClient client = new(EVENT_HUB_CONNECTION_STRING_PLAIN, "StockTickerData");
            using EventDataBatch batch = await client.CreateBatchAsync();
            batch.TryAdd(new EventData(Encoding.UTF8.GetBytes(st.ToString())));
            await client.SendAsync(batch);
            _logger.LogInformation($"Stock data sent to Event Hub: {st}");

            return new OkResult();
        }
    }
}
