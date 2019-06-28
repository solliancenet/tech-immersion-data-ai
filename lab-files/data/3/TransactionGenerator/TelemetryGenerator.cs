using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using TechImmersion.Common.Models;

namespace TransactionGenerator
{
    public static class TelemetryGenerator
    {
        private static readonly Random Random = new Random();
        private const double HighSpeedProbabilityPower = 0.3;
        private const double LowSpeedProbabilityPower = 0.9;
        private const double HighOilProbabilityPower = 0.3;
        private const double LowOilProbabilityPower = 1.2;
        private const double HighTirePressureProbabilityPower = 0.5;
        private const double LowTirePressureProbabilityPower = 1.7;
        private const double HighOutsideTempProbabilityPower = 0.3;
        private const double LowOutsideTempProbabilityPower = 1.2;
        private const double HighEngineTempProbabilityPower = 0.3;
        private const double LowEngineTempProbabilityPower = 1.2;
        static readonly List<string> VinList = new List<string>();

        public static void Init()
        {
            GetVinMasterList();
        }

        public static CarEvent GenerateMessage()
        {
            var city = GetLocation();
            return new CarEvent()
            {
                vin = GetRandomVin(),
                city = city,
                outsideTemperature = GetOutsideTemp(city),
                engineTemperature = GetEngineTemp(city),
                speed = GetSpeed(city),
                fuel = Random.Next(0, 40),
                engineoil = GetOil(city),
                tirepressure = GetTirePressure(city),
                odometer = Random.Next(0, 200000),
                accelerator_pedal_position = Random.Next(0, 100),
                parking_brake_status = GetRandomBoolean(),
                headlamp_status = GetRandomBoolean(),
                brake_pedal_status = GetRandomBoolean(),
                transmission_gear_position = GetGearPos(),
                ignition_status = GetRandomBoolean(),
                windshield_wiper_status = GetRandomBoolean(),
                abs = GetRandomBoolean(),
                timestamp = DateTime.UtcNow
            };
        }

        private static void OnAsyncMethodFailed(Task task)
        {
            System.Console.WriteLine(task.Exception?.ToString() ?? "null error");
        }

        private static int GetSpeed(string city)
        {
            if (city.ToLower() == "orlando")
            {
                return GetRandomWeightedNumber(100, 0, HighSpeedProbabilityPower);
            }
            return GetRandomWeightedNumber(100, 0, LowSpeedProbabilityPower);
        }

        public static int GetOil(string city)
        {
            if (city.ToLower() == "orlando")
            {
                return GetRandomWeightedNumber(50, 0, LowOilProbabilityPower);
            }
            return GetRandomWeightedNumber(50, 0, HighOilProbabilityPower);
        }

        public static int GetTirePressure(string city)
        {
            if (city.ToLower() == "orlando")
            {
                return GetRandomWeightedNumber(50, 0, LowTirePressureProbabilityPower);
            }
            return GetRandomWeightedNumber(50, 0, HighTirePressureProbabilityPower);
        }

        public static int GetEngineTemp(string city)
        {
            if (city.ToLower() == "orlando")
            {
                return GetRandomWeightedNumber(500, 0, HighEngineTempProbabilityPower);
            }
            return GetRandomWeightedNumber(500, 0, LowEngineTempProbabilityPower);
        }

        public static int GetOutsideTemp(string city)
        {
            if (city.ToLower() == "orlando")
            {
                return GetRandomWeightedNumber(100, 0, LowOutsideTempProbabilityPower);
            }
            return GetRandomWeightedNumber(100, 0, HighOutsideTempProbabilityPower);
        }
        private static int GetRandomWeightedNumber(int max, int min, double probabilityPower)
        {
            var randomizer = new Random();
            var randomDouble = randomizer.NextDouble();

            var result = Math.Floor(min + (max + 1 - min) * (Math.Pow(randomDouble, probabilityPower)));
            return (int)result;
        }

        public static string GetGearPos()
        {
            var list = new List<string>() { "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eight" };
            var l = list.Count;
            var r = new Random();
            var num = r.Next(l);
            return list[num];
        }

        public static bool GetRandomBoolean()
        {
            return new Random().Next(100) % 2 == 0;
        }

        private static void GetVinMasterList()
        {
            var reader = new StreamReader(File.OpenRead(@"VINMasterList.csv"));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == null) continue;
                var values = line.Split(';');

                VinList.Add(values[0]);
            }
        }

        private static string GetRandomVin()
        {
            var randomIndex = Random.Next(1, VinList.Count - 1);
            return VinList[randomIndex];
        }

        public static string GetLocation()
        {
            var list = new List<string>() { "Los Angeles", "San Diego", "Chicago", "Madison", "Orlando", "Tampa" };
            var l = list.Count;
            var r = new Random();
            var num = r.Next(l);
            return list[num];
        }
    }
}
