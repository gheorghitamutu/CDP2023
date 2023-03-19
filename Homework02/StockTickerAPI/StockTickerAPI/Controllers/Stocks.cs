using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using System.Text;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json.Linq;

namespace StockTickerAPI.Controllers
{
    [ApiController]
    [Route("/get_stocks")]
    public class Stocks : ControllerBase
    {
        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string? EndpointUri = System.Configuration.ConfigurationManager.AppSettings["EndPointUri"];

        // The primary key for the Azure Cosmos account.
        private static readonly string? PrimaryKey = System.Configuration.ConfigurationManager.AppSettings["PrimaryKey"];

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

        private readonly ILogger<Stocks> _logger;

        public Stocks(ILogger<Stocks> logger) => _logger = logger;

        [HttpGet(Name = "GetStockTicker")]
        public async Task<IEnumerable<StockTicker>> Get()
        {
            await Initialize();

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

            return stocks;
        }

        public async Task Initialize()
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

        [ApiController]
        [Route("/set_stock")]
        public class SetStock : ControllerBase
        {
            private readonly static string? EVENT_HUB_CONNECTION_STRING = Environment.GetEnvironmentVariable("EventHub_stockticker_1679176237014");
            // private readonly static string? BLOB_STORAGE_CONNECTION_STRING = Environment.GetEnvironmentVariable("BlobStorage_stockticker");
            const string EVENT_HUB_CONNECTION_STRING_PLAIN = "Endpoint=sb://stock-ticker.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Q6Xjs7/o1044U2SH/RwKNB4UbcWuzPfe6+AEhAdW6uQ=";

            [FunctionName("SetStock")]
            public static async Task<Microsoft.AspNetCore.Mvc.IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "/sendStockData")] HttpRequest req,
            [ServiceBus("StockTickerData", Connection = EVENT_HUB_CONNECTION_STRING_PLAIN)] IAsyncCollector<string> eventHubMessages,
            ILogger log)
            {
                log.LogInformation($"StockTickerFunction HTTP trigger function started {req}.");

                if (req.ContentType == "application/json")
                {
                    var data = JObject.Parse(req.Body.ToString());

                    string symbol = data["Symbol"].ToString();
                    double price = double.Parse(data["Price"].ToString());
                    int volume = int.Parse(data["Volume"].ToString());
                    double change = double.Parse(data["Change"].ToString());
                    double changePercent = double.Parse(data["ChangePercent"].ToString());
                    DateTime date = DateTime.Parse(data["Date"].ToString());

                    // Send stock data to Event Hub
                    var dataString = $"{symbol},{price},{volume},{change},{changePercent},{date}";
                    await eventHubMessages.AddAsync(dataString);
                    log.LogInformation($"Stock data sent to Event Hub: {dataString}");

                    return new OkResult();
                }

                return new BadRequestResult();
            }
        }
    }
}
