using System.Collections.Generic;
using System.Linq;
using IoTProcessing.DistanceCalculator;
using IoTProcessing.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;

namespace IoTProcessing;
public class AnomalyDetector
{
    private const double MAX_TEMPERATURE = 50;
    private const double MAX_DISTANCE_KM = 40;
    private const double MIN_HEART_RATE = 30;

    private List<Anomaly> GetAnomalies(List<DeviceIdAndSensorsData> observations)
    {
        var anomalies = new List<Anomaly>();

        // Check for body temperature anomalies
        foreach (var obs in observations)
        {
            if (obs.body_temperature > MAX_TEMPERATURE)
            {
                anomalies.Add(new Anomaly
                {
                    name = "High body temperature",
                    description = $"Body temperature is higher than {MAX_TEMPERATURE} degrees Celsius",
                    deviceId = obs.deviceId,
                    timestamp = obs.timestamp,
                    observationsWhereHappened = new List<DeviceIdAndSensorsData> { obs }
                });
            }
        }

        // Check for distance anomalies
        for (int obsIndex = 0; obsIndex < observations.Count; obsIndex++)
        {

            for (int obsIndex2 = obsIndex + 1; obsIndex2 < observations.Count; obsIndex2++)
            {
                var obs = observations[obsIndex];
                var nextObs = observations[obsIndex2];

                var distanceInKilometers = DistanceCalculatorUtil.DistanceTo(obs.location.latitude, obs.location.longitude, nextObs.location.latitude, nextObs.location.longitude);

                if (distanceInKilometers > MAX_DISTANCE_KM)
                {
                    var anomaly = new Anomaly
                    {
                        name = "Livestock left the farm",
                        description = $"Livestock is outside the farm (at least {MAX_DISTANCE_KM} km away from the farm)",
                        deviceId = obs.deviceId,
                        timestamp = obs.timestamp,
                        observationsWhereHappened = new List<DeviceIdAndSensorsData> { obs, nextObs }
                    };

                    anomalies.Add(anomaly);

                    return anomalies;
                }
            }
        }

        // Check for heart rate anomalies
        foreach (var obs in observations)
        {
            if (obs.heart_rate < MIN_HEART_RATE)
            {
                anomalies.Add(new Anomaly
                {
                    name = "Low heart rate",
                    description = $"Heart rate lower than {MIN_HEART_RATE} bpm",
                    deviceId = obs.deviceId,
                    timestamp = obs.timestamp,
                    observationsWhereHappened = new List<DeviceIdAndSensorsData> { obs }
                });
            }
        }

        return anomalies;
    }


    [FunctionName("AnomalyDetectorFunction")]
    public void AnomalyDetectorFunction([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer,
    [CosmosDB(databaseName: "StocksDb", containerName: "LivestockIoTData", Connection = "CosmosDbConnectionString")] IAsyncCollector<object> newMessagesOut,
    [CosmosDB(databaseName: "StocksDb", containerName: "LivestockIoTData", Connection = "CosmosDbConnectionString", SqlQuery = "SELECT * FROM c WHERE not IS_DEFINED(c.processed) or c.processed = false ORDER BY c.timestamp")] IEnumerable<DeviceIdAndSensorsData> newMessages,
    [ServiceBus("anomalies", entityType: ServiceBusEntityType.Queue, Connection = "ServiceBusConnectionString")] IAsyncCollector<AnomaliesForDevice> anomaliesOut,
     ILogger log)
    {
        // group by deviceId
        var groupedMessages = newMessages.GroupBy(x => x.deviceId)
                                        .Select(g => g.ToList())
                                        .ToList();

        foreach (var group in groupedMessages)
        {
            var anomalies = GetAnomalies(group);

            if (anomalies.Count > 0)
            {
                log.LogInformation($"Anomaly detected");

                anomaliesOut.AddAsync(new AnomaliesForDevice
                {
                    deviceId = group[0].deviceId,
                    anomalies = anomalies
                });
            }
        }

        // mark messages as processed
        foreach (var message in newMessages)
        {
            message.processed = "true";
            newMessagesOut.AddAsync(message);
        }

    }
}
