using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using TechImmersion.Common;
using TechImmersion.Common.Models;

namespace TechImmersion.CarEventProcessor
{
    public class TelemetryProcessing
    {
        public async Task ProcessEvent(CarEvent carEventData, Dictionary<string, string> cityRegionMap,
            IAsyncCollector<EventData> outputEventHubData)
        {
            if (cityRegionMap.ContainsKey(carEventData.city)) carEventData.region = cityRegionMap[carEventData.city];
            else
            {
                throw new InvalidOperationException($"Could not find a region mapped to the city: {carEventData.city}");
            }
            // Serialize the CarEvent object, add it to a new EventData object, then add to the EventData collection.
            var eventData = new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(carEventData)));
            await outputEventHubData.AddAsync(eventData);
        }

        public Dictionary<string, string> GetCityRegionMap()
        {
            var cityRegionMap = new Dictionary<string, string>
            {
                // For demo purposes, just returning a predefined dictionary vs. retrieving the values.
                { "Los Angeles", "Southwest" },
                { "San Diego", "Southwest" },
                { "Chicago", "Central" },
                { "Madison", "Central" },
                { "Orlando", "Southeast" },
                { "Tampa", "Southeast" }
            };

            /*var reader = new StreamReader(File.OpenRead(@"CityRegionMap.csv"));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == null) continue;
                var values = line.Split(',');

                cityRegionMap.Add(values[0], values[1]);
            }*/

            return cityRegionMap;
        }
    }
}
