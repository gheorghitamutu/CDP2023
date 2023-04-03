using Livestock;
using System;
using System.IO;
using System.Text;
using Google.Protobuf;
using System.Text.Json;

namespace SimulatedDeviceProtobuf
{
    public class ProtobufDataGenerator
    {
        const string animalsTemperatureAnomaliesFile = "animalsTemperatureAnomalies";
        const string animalsLocationAnomaliesFile = "animalsLocationAnomalies";
        const string animalsHeartRateAnomaliesFile = "animalsHeartRateAnomalies";
        const string animalsNormalFile = "animalsNormal";

        Animals animalsTemperatureAnomalies;
        Animals animalsLocationAnomalies;
        Animals animalsHeartRateAnomalies;
        Animals animalsNormal;

        public Animals AnimalsTemperatureAnomalies
        {
            get { return animalsTemperatureAnomalies; }
        }

        public Animals AnimalsLocationAnomalies
        {
            get { return animalsLocationAnomalies; }
        }

        public Animals AnimalsHeartRateAnomalies
        {
            get { return animalsHeartRateAnomalies; }
        }

        public Animals AnimalsNormal
        {
            get { return animalsNormal; }
        }

        public ProtobufDataGenerator()
        {
            animalsNormal = GenerateAnimalsInNormalConditions();
            animalsHeartRateAnomalies = GenerateAnimalsHeartRateAnomalies();
            animalsLocationAnomalies = GenerateAnimalsLocationAnomalies();
            animalsTemperatureAnomalies = GenerateAnimalsTemperatureAnomalies();
        }

        public static Animals DeserializeToData(string protodatPath)
        {
            using var stream = File.OpenRead(protodatPath);
            return Animals.Parser.ParseFrom(stream);

        }

        public static void SerializeToFile(string protodatPath, Animals lds)
        {
            using var output = File.Create(protodatPath);
            lds.WriteTo(output);
        }

        static public double GetRandomDouble(Random random, double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        public static Animals GenerateAnimalsInNormalConditions()
        {
            Animals animals = new();

            Random randomGenerator = new();

            var animalsNo = randomGenerator.Next(2, 10);
            var animalEntriesNo = randomGenerator.Next(2, 10);

            for (var i = 0; i < animalsNo; i++)
            {
                string id = randomGenerator.Next(100, 1000).ToString();
                double latitude = GetRandomDouble(randomGenerator, 10.0, 50.0);
                double longitude = GetRandomDouble(randomGenerator, 10.0, 50.0);
                double weight = GetRandomDouble(randomGenerator, 200.0, 500.0);

                for (var j = 0; j < animalEntriesNo; j++)
                {
                    animals.Entries.Add(
                        new Animal
                        {
                            AnimalId = id,
                            BodyTemperature = GetRandomDouble(randomGenerator, 28.0, 50.0),
                            ActivityLevel = randomGenerator.NextDouble(),
                            HeartRate = randomGenerator.Next(60, 100),
                            RespirationRate = randomGenerator.Next(10, 30),
                            Location = new Location { Latitude = latitude, Longitude = longitude },
                            FeedIntake = GetRandomDouble(randomGenerator, 2.0, 10.0),
                            WaterConsumption = GetRandomDouble(randomGenerator, 2.0, 10.0),
                            MilkProduction = GetRandomDouble(randomGenerator, 2.0, 10.0),
                            Weight = weight
                        });

                    latitude += 5.0;
                    longitude += 5.0;
                }
            }

            return animals;
        }

        public static Animals GenerateAnimalsHeartRateAnomalies()
        {
            Animals animals = new();

            Random randomGenerator = new();

            var animalsNo = randomGenerator.Next(2, 10);
            var animalEntriesNo = randomGenerator.Next(2, 10);

            for (var i = 0; i < animalsNo; i++)
            {
                string id = randomGenerator.Next(100, 1000).ToString();
                double latitude = GetRandomDouble(randomGenerator, 10.0, 50.0);
                double longitude = GetRandomDouble(randomGenerator, 10.0, 50.0);
                double weight = GetRandomDouble(randomGenerator, 200.0, 500.0);

                for (var j = 0; j < animalEntriesNo; j++)
                {
                    animals.Entries.Add(
                        new Animal
                        {
                            AnimalId = id,
                            BodyTemperature = GetRandomDouble(randomGenerator, 28.0, 50.0),
                            ActivityLevel = randomGenerator.NextDouble(),
                            HeartRate = randomGenerator.Next(0, 40),  // minimum is 30
                            RespirationRate = randomGenerator.Next(10, 30),
                            Location = new Location { Latitude = latitude, Longitude = longitude },
                            FeedIntake = GetRandomDouble(randomGenerator, 2.0, 10.0),
                            WaterConsumption = GetRandomDouble(randomGenerator, 2.0, 10.0),
                            MilkProduction = GetRandomDouble(randomGenerator, 2.0, 10.0),
                            Weight = weight
                        });

                    latitude += 5.0;
                    longitude += 5.0;
                }
            }

            return animals;
        }

        public static Animals GenerateAnimalsLocationAnomalies()
        {
            Animals animals = new();

            Random randomGenerator = new();

            var animalsNo = randomGenerator.Next(2, 10);
            var animalEntriesNo = randomGenerator.Next(2, 10);

            for (var i = 0; i < animalsNo; i++)
            {
                string id = randomGenerator.Next(100, 1000).ToString();
                double latitude = GetRandomDouble(randomGenerator, 10.0, 50.0);
                double longitude = GetRandomDouble(randomGenerator, 10.0, 50.0);
                double weight = GetRandomDouble(randomGenerator, 200.0, 500.0);

                for (var j = 0; j < animalEntriesNo; j++)
                {
                    animals.Entries.Add(
                        new Animal
                        {
                            AnimalId = id,
                            BodyTemperature = GetRandomDouble(randomGenerator, 28.0, 50.0),
                            ActivityLevel = randomGenerator.NextDouble(),
                            HeartRate = randomGenerator.Next(60, 100),
                            RespirationRate = randomGenerator.Next(10, 30),
                            Location = new Location { Latitude = latitude, Longitude = longitude },
                            FeedIntake = GetRandomDouble(randomGenerator, 2.0, 10.0),
                            WaterConsumption = GetRandomDouble(randomGenerator, 2.0, 10.0),
                            MilkProduction = GetRandomDouble(randomGenerator, 2.0, 10.0),
                            Weight = weight
                        });

                    latitude += 50.0;
                    longitude += 50.0;
                }
            }

            return animals;
        }

        public static Animals GenerateAnimalsTemperatureAnomalies()
        {
            Animals animals = new();

            Random randomGenerator = new();

            var animalsNo = randomGenerator.Next(2, 10);
            var animalEntriesNo = randomGenerator.Next(2, 10);

            for (var i = 0; i < animalsNo; i++)
            {
                string id = randomGenerator.Next(100, 1000).ToString();
                double latitude = GetRandomDouble(randomGenerator, 10.0, 50.0);
                double longitude = GetRandomDouble(randomGenerator, 10.0, 50.0);
                double weight = GetRandomDouble(randomGenerator, 200.0, 500.0);

                for (var j = 0; j < animalEntriesNo; j++)
                {
                    animals.Entries.Add(
                        new Animal
                        {
                            AnimalId = id,
                            BodyTemperature = GetRandomDouble(randomGenerator, 40.0, 80.0), // maximum is 50
                            ActivityLevel = randomGenerator.NextDouble(),
                            HeartRate = randomGenerator.Next(60, 100),
                            RespirationRate = randomGenerator.Next(10, 30),
                            Location = new Location { Latitude = latitude, Longitude = longitude },
                            FeedIntake = GetRandomDouble(randomGenerator, 2.0, 10.0),
                            WaterConsumption = GetRandomDouble(randomGenerator, 2.0, 10.0),
                            MilkProduction = GetRandomDouble(randomGenerator, 2.0, 10.0),
                            Weight = weight
                        });

                    latitude += 5.0;
                    longitude += 5.0;
                }
            }

            return animals;
        }

        static public void SerializeToJSON(string filepath, Animals animals)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(animals, options);

            using var output = File.Create(filepath);
            output.Write(Encoding.UTF8.GetBytes(jsonString));
        }

        public void SerializeToFile()
        {
            SerializeToFile(animalsNormalFile + ".protodat", animalsNormal);
            SerializeToFile(animalsHeartRateAnomaliesFile + ".protodat", animalsHeartRateAnomalies);
            SerializeToFile(animalsLocationAnomaliesFile + ".protodat", animalsLocationAnomalies);
            SerializeToFile(animalsTemperatureAnomaliesFile + ".protodat", animalsTemperatureAnomalies);

            SerializeToJSON(animalsNormalFile + ".json", animalsNormal);
            SerializeToJSON(animalsHeartRateAnomaliesFile + ".json", animalsHeartRateAnomalies);
            SerializeToJSON(animalsLocationAnomaliesFile + ".json", animalsLocationAnomalies);
            SerializeToJSON(animalsTemperatureAnomaliesFile + ".json", animalsTemperatureAnomalies);
        }

        public Animals DeserializeFromFile(string filepath)
        {
            return DeserializeToData(filepath);
        }
    }
}
