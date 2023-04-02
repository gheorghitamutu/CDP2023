using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IoTProcessing
{
    public static class IoTMessageProcessor
    {
        [FunctionName("IoTMessageProcessor")]
        public static async Task Run([EventHubTrigger("iothub-ehub-livestockm-24895990-3d3480a3f5", Connection = "IotEventHubConnectionString")] EventData[] events,
         [CosmosDB(databaseName: "StocksDb", containerName: "LivestockIoTData", Connection = "CosmosDbConnectionString")] IAsyncCollector<object> documents,
         ILogger log)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.ToArray());

                    string deviceId = eventData.SystemProperties["iothub-connection-device-id"].ToString();
                    string enqueuedTime = eventData.SystemProperties["iothub-enqueuedtime"].ToString();

                    dynamic message = JsonConvert.DeserializeObject(messageBody);

                    //convert enqueue time to unix timestamp. Enqueued time has format example: 30.03.2023 22:52:30
                    var timestamp = DateTimeOffset.Parse(enqueuedTime).ToUnixTimeSeconds().ToString();


                    // de luat data exacta
                    message.deviceId = deviceId;
                    message.id = Guid.NewGuid().ToString();
                    message.dateTime = enqueuedTime;
                    message.timestamp = timestamp;

                    await documents.AddAsync(message);
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
