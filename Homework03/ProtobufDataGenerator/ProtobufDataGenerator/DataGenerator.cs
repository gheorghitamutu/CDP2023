using System;
using Livestock;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SimulatedDeviceProtobuf;

namespace ProtobufData
{
    public class DataGenerator
    {
        private static async Task SendDeviceToCloudMessagesAsync(DeviceClient iotDevice, Animals animals)
        {

            foreach (var animal in animals.Entries)
            {
                // Create JSON message
                var message = new Message(Encoding.UTF8.GetBytes(animal.ToString()))
                {
                    // Add a custom application property to the message.
                    // An IoT hub can filter on these properties without access to the message body.
                    // message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");
                    ContentType = "application/json",
                    ContentEncoding = "utf-8"
                };

                // Send the tlemetry message
                await iotDevice.SendEventAsync(message).ConfigureAwait(false);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, animal.ToString());

                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            }
        }

        [FunctionName("DataGenerator")]
        public static async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer,
            ILogger log)
        {
            DeviceClient iotDevice =
                DeviceClient.CreateFromConnectionString(
                    "HostName=LivestockMonitoring.azure-devices.net;DeviceId=TestDevice;SharedAccessKey=Y8g72kev20uR5IXC4O+Nr/TgVN5QXtj7WniDXNG3cWw=");

            ProtobufDataGenerator protobufDataGenerator = new();

            await SendDeviceToCloudMessagesAsync(iotDevice, protobufDataGenerator.AnimalsNormal);
            log.LogInformation($"Data sent to IoT Event Hub: {protobufDataGenerator.AnimalsNormal} at: {DateTime.Now}");

            await SendDeviceToCloudMessagesAsync(iotDevice, protobufDataGenerator.AnimalsHeartRateAnomalies);
            log.LogInformation($"Data sent to IoT Event Hub: {protobufDataGenerator.AnimalsHeartRateAnomalies} at: {DateTime.Now}");

            await SendDeviceToCloudMessagesAsync(iotDevice, protobufDataGenerator.AnimalsLocationAnomalies);
            log.LogInformation($"Data sent to IoT Event Hub: {protobufDataGenerator.AnimalsLocationAnomalies} at: {DateTime.Now}");

            await SendDeviceToCloudMessagesAsync(iotDevice, protobufDataGenerator.AnimalsTemperatureAnomalies);
            log.LogInformation($"Data sent to IoT Event Hub: {protobufDataGenerator.AnimalsTemperatureAnomalies} at: {DateTime.Now}");

        }
    }
}
