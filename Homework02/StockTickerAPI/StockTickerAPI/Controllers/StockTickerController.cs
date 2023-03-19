using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using System.Text;
using Microsoft.Azure.Cosmos;


namespace StockTickerAPI.Controllers
{
    [ApiController]
    [Route("/get_stocks")]
    public class StockTickerController : ControllerBase
    {
        // private readonly static string? EVENT_HUB_CONNECTION_STRING = Environment.GetEnvironmentVariable("EventHub_stockticker_1679176237014");
        // private readonly static string? BLOB_STORAGE_CONNECTION_STRING = Environment.GetEnvironmentVariable("BlobStorage_stockticker");

        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string? EndpointUri = System.Configuration.ConfigurationManager.AppSettings["EndPointUri"];

        // The primary key for the Azure Cosmos account.
        private static readonly string? PrimaryKey = System.Configuration.ConfigurationManager.AppSettings["PrimaryKey"];

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private readonly string databaseId = "StockEntries";
        private readonly string containerId = "Items";

        // // Create a blob container client that the event processor will use 
        // private readonly static BlobContainerClient storageClient = new(BLOB_STORAGE_CONNECTION_STRING, "stockticker");
        // 
        // // Create an event processor client to process events in the event hub
        // private readonly static EventProcessorClient processor = new(
        //     storageClient, EventHubConsumerClient.DefaultConsumerGroupName, EVENT_HUB_CONNECTION_STRING, "stockticker_1679176237014");

        private readonly ILogger<StockTickerController> _logger;

        public StockTickerController(ILogger<StockTickerController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetStockTicker")]
        public async Task<IEnumerable<StockTicker>> Get()
        {
            await Initialize();

            var sqlQueryText = "SELECT * FROM c";
            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new(sqlQueryText);
            FeedIterator<StockTicker> queryResultSetIterator = container.GetItemQueryIterator<StockTicker>(queryDefinition);

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
                    Console.WriteLine("Current provisioned throughput : {0}\n", throughput.Value);
                    int newThroughput = throughput.Value + 100;
                    // Update throughput
                    await container.ReplaceThroughputAsync(newThroughput);
                    Console.WriteLine("New provisioned throughput : {0}\n", newThroughput);
                }
            }
            catch (CosmosException cosmosException) when (cosmosException.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Console.WriteLine("Cannot read container throuthput.");
                Console.WriteLine(cosmosException.ResponseBody);
            }

            // try
            // {
            //     // Read the item to see if it exists
            //     ItemResponse<Family> wakefieldFamilyResponse = await this.container.ReadItemAsync<Family>(wakefieldFamily.Id, new PartitionKey(wakefieldFamily.PartitionKey));
            //     Console.WriteLine("Item in database with id: {0} already exists\n", wakefieldFamilyResponse.Resource.Id);
            // }
            // catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            // {
            //     // Create an item in the container representing the Wakefield family. Note we provide the value of the partition key for this item, which is "Wakefield"
            //     ItemResponse<Family> wakefieldFamilyResponse = await this.container.CreateItemAsync<Family>(wakefieldFamily, new PartitionKey(wakefieldFamily.PartitionKey));
            // 
            //     // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
            //     Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", wakefieldFamilyResponse.Resource.Id, wakefieldFamilyResponse.RequestCharge);
            // }

            // await this.ReplaceFamilyItemAsync();
            // await this.DeleteFamilyItemAsync();
            // await this.DeleteDatabaseAndCleanupAsync();
        }


    }
}
