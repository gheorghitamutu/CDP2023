// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// This application uses the Azure IoT Hub device SDK for .NET
// For samples see: https://github.com/Azure/azure-iot-sdk-csharp/tree/master/iothub/device/samples

using System;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using IoTProcessing.Models;
using System.IO;
using System.Linq;

namespace IoTProcessing
{
    class SimulatedDevice
    {
        private static DeviceClient s_deviceClient;

        // NU trimite mai mult de atatea chiar daca mai sunt obs. in lista citite din fisier
        public static int MESSAGES_LIMIT { get; set; } = 10;
        public static TimeSpan MESSAGE_SEND_INTERVAL { get; set; } = TimeSpan.FromSeconds(1);


        // The device connection string to authenticate the device with your IoT hub.
        private const string s_connectionString_device01 = "HostName=livestockMonitorsIotHub.azure-devices.net;DeviceId=healthSec01;SharedAccessKey=4exqjIvWuFd/KsLY0sbScxdfv+1ptRqsqvplFHrjZU8=";
        private const string s_connectionString_device02 = "HostName=livestockMonitorsIotHub.azure-devices.net;DeviceId=healthSec02;SharedAccessKey=0fC6vInfU+vInlrAuvSIUIL1vOm27X4zKpk9uN/zNHM=";


        private static async Task<List<DeviceSensorsData>> ReadMessagesFromFileAsync(string path)
        {
            var observations = new List<DeviceSensorsData>();

            var observationsFromFile = await File.ReadAllTextAsync(path);

            observations = JsonConvert.DeserializeObject<List<DeviceSensorsData>>(observationsFromFile);

            return observations;
        }

        // Async method to send simulated telemetry
        private static async Task SendDeviceToCloudMessagesAsync(List<DeviceSensorsData> observationsToSend)
        {

            foreach (var observation in observationsToSend.Take(MESSAGES_LIMIT))
            {
                // Create JSON message
                var messageString = JsonConvert.SerializeObject(observation);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                // Add a custom application property to the message.
                // An IoT hub can filter on these properties without access to the message body.
                // message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");
                message.ContentType = "application/json";
                message.ContentEncoding = "utf-8";

                // Send the tlemetry message
                await s_deviceClient.SendEventAsync(message).ConfigureAwait(false);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(MESSAGE_SEND_INTERVAL).ConfigureAwait(false);
            }
        }

        private static async Task Main()
        {
            Console.WriteLine("IoT Hub Quickstarts - Simulated device. Ctrl-C to exit.\n");

            // conditii normale
            // var observations = await ReadMessagesFromFileAsync("./data/normal_conditions_obs.json");

            // cu anomalii de spatiu (primele 5 intrari au o locatie, celelalte alta locatie cu macar 80 km diferenta)
            var observations = await ReadMessagesFromFileAsync("./data/location_anomaly_obs.json");

            // cu valori anormale de temperatura (ultimele intrari au valori de temperatura anormale - au > 50 for example)
            // var observations = await ReadMessagesFromFileAsync("./data/anormal_temperatures_obs.json");

            // cu valori anormale de heart rate (anormal = valori < 30 hardcoded in AnomalyDetector)
            //var observations = await ReadMessagesFromFileAsync("./data/abnormal_heart_rate_obs.json");

            // Connect to the IoT hub using the MQTT protocol
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString_device01, TransportType.Mqtt);
            await SendDeviceToCloudMessagesAsync(observations);
            Console.ReadLine();
        }
    }
}
