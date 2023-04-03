using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventHubs;
using System.Threading.Tasks;
using Livestock;
using System;
using System.Collections.Generic;

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

            if (animal.HeartRate > MIN_HEART_RATE)
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
            [CosmosDB(databaseName: "livestocknosql", containerName: "SensorsDataAnimals", Connection = "COSMOS_DB_CONNECTION_STRING")] IAsyncCollector<object> documents,
            [CosmosDB(databaseName: "livestocknosql", containerName: "SensorsDataAnimals", Connection = "COSMOS_DB_CONNECTION_STRING", SqlQuery = "SELECT TOP 1 * FROM c ORDER BY c._ts DESC")] IEnumerable<Animal> animals,
            ILogger log)
        {
            var body = message.EventBody.ToString();
            var animal = Animal.Parser.ParseJson(body);

            Animal previousAnimal = null;
            foreach (var previous in animals)
            {
                if (previous.AnimalId == animal.AnimalId)
                {
                    previousAnimal = previous;
                }
            }
            DetectAnomaliesQueueingAlerts(animal, previousAnimal);
            
            await documents.AddAsync(body);

            log.LogInformation($"C# IoT Hub trigger function processed a message: {animal}");
        }
    }
}