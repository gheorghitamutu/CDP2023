using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventHubs;
using System.Threading.Tasks;
using Livestock;
using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace SensorDataProcessor
{
    public class DataProcessor
    {
        private const double MAX_TEMPERATURE = 50;
        private const double MAX_DISTANCE_KM = 40;
        private const double MIN_HEART_RATE = 30;

        public static double DistanceTo(Location l1, Location l2, char unit = 'K')
        {
            double rlat1 = Math.PI * l1.Latitude / 180;
            double rlat2 = Math.PI * l2.Latitude / 180;
            double theta = l1.Longitude - l2.Longitude;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }

        public void DetectAnomaliesQueueingAlerts(Animal animal, Animal previousEntryAnimal)
        {
            if (animal.BodyTemperature > MAX_TEMPERATURE)
            {

            }

            if (animal.HeartRate < MIN_HEART_RATE)
            {

            }

            if (previousEntryAnimal != null)
            {
                if (DistanceTo(animal.Location, previousEntryAnimal.Location) > MAX_DISTANCE_KM)
                {

                }
            }
        }

        [FunctionName("ProcessData")]
        public async Task Run(
            [EventHubTrigger("LivestockMonitoring", Connection = "IOT_HUB_CONNECTION_STRING")] EventData message,
            ILogger log)
        {
            var body = message.EventBody.ToString();
            var animal = Animal.Parser.ParseJson(body);

            // The Azure Cosmos DB endpoint for running this sample.
            string EndpointUri = "https://livestocknosql.documents.azure.com:443/";

            // The primary key for the Azure Cosmos account.
            string PrimaryKey = "n9k8tbhcCB9APopbKAplQoK4373bWfX0RrXkEV2L8sHuaz0WFCOjloiFfi0XFp7lMOrR2MgBQJ34ACDbLkudfA==";

            // The name of the database and container we will create
            string databaseId = "SensorDataAnimals";
            string containerId = "Items";

            CosmosClient cosmosClient = new(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "SensorDataProcessor" });
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");

            QueryDefinition queryDefinition = new("SELECT TOP 1 * FROM c ORDER BY c._ts DESC");
            FeedIterator<Animal> queryResultSetIterator = container?.GetItemQueryIterator<Animal>(queryDefinition);

            List<Animal> animals = new();
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Animal> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Animal resultAnimal in currentResultSet)
                {
                    animals.Add(resultAnimal);
                }
            }

            Animal previousAnimal = null;
            foreach (var obj in animals)
            {
                if (obj.AnimalId == animal.AnimalId)
                {
                    previousAnimal = obj;
                }
            }
            DetectAnomaliesQueueingAlerts(animal, previousAnimal);

            try
            {
                container = await database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");
                var payload = JsonConvert.DeserializeObject(body);
                ItemResponse<object> response = await container.CreateItemAsync(payload);

                log.LogInformation($"Added item in database: {body}.");
            }
            catch (CosmosException ex)
            {
                log.LogError($"Failed: {ex.ResponseBody}.");
                log.LogError($"Failed: {animal}.");
            }

            log.LogInformation($"C# IoT Hub trigger function processed a message: {animal}");

            cosmosClient.Dispose();
        }
    }
}