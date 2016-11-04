using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace FraudDevice01
{
    class Program
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "호스트주소.azure-devices.net";
        static string deviceKey = "디바이스키";

        private static async void SendDeviceToCloudMessagesAsync()
        {
            //double avgWindSpeed = 10; // m/s
            Random rand = new Random();

            int i = 0;
            //while (true)
            while (i <= 50)
            {
                //double currentWindSpeed = avgWindSpeed + rand.NextDouble() * 4 - 2;
                int level = rand.Next(0, 100);
                int points = rand.Next(0, 50);
                int winnerYN = 0;
                //float prediction = 0;

                var telemetryDataPoint = new
                {
                    deviceId = "cb2-fraud-device",
                    DateTime.Now,
                    level,
                    points,
                    winnerYN
                    //prediction
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                Task.Delay(1).Wait();
                i++;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("cb2-fraud-device", deviceKey));

            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }
    }
}
