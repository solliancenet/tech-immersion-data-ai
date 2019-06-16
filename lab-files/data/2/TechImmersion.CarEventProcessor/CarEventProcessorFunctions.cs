using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TechImmersion.CarEventProcessor.Helpers;
using TechImmersion.Common;
using TechImmersion.Common.Models;

namespace TechImmersion.CarEventProcessor
{
    public static class CarEventProcessorFunctions
    {
        private static Dictionary<string, string> _cityRegionMap = null;

        /// <summary>
        /// The CarEventProcessor function is triggered by a Cosmos DB change feed as
        /// documents are written to the telemetry collection. This function performs
        /// some minor processing of that data by adding a region based on the city,
        /// then sends the enriched data to Event Hubs in batches for further processing.
        /// 
        /// It is configured to read from just one region to demonstrate how to read data
        /// from a specific Cosmos DB region.
        /// </summary>
        /// <param name="input">The Cosmos DB documents sent by the change feed.</param>
        /// <param name="eventHubOutput">The collection of Event Hubs events to send with
        /// enriched car telemetry data.</param>
        /// <param name="log">The logger used for outputting information and errors to the
        /// functions log.</param>
        /// <returns></returns>
        [FunctionName("CarEventProcessorRegion1")]
        public static async Task CarEventProcessorRegion1([CosmosDBTrigger(
            databaseName: "ContosoAuto",
            collectionName: "telemetry",
            // Uses the "Region1" application configuration value to pass in the region 1 name:
            PreferredLocations = "%Region1%",
            ConnectionStringSetting = "CosmosDbConnectionString",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            [EventHub("telemetry",
                Connection="EventHubsConnectionString")]IAsyncCollector<EventData> eventHubOutput,
            ILogger log)
        {
            log.LogInformation($"Cosmos DB processor received {input.Count} documents from Region 1");
            var telemetryProcessing = new TelemetryProcessing();
            if (_cityRegionMap == null) _cityRegionMap = telemetryProcessing.GetCityRegionMap();

            // Parse the incoming messages and get the Region from the City.
            // Default value is blank. Blank regions can be fixed up later in a downstream process.
            // Route data to Event Hubs.
            foreach (var carData in input)
            {
                try
                {
                    if (carData != null)
                    {
                        if (!IsTelemetryData(carData)) continue; // Skip if this document is not telemetry data.
                        var carEventData = await carData.ReadAsAsync<CarEvent>();
                        await telemetryProcessing.ProcessEvent(carEventData,
                            _cityRegionMap, eventHubOutput);
                    }
                }
                catch (Exception ex)
                {
                    log.LogError("Cosmos DB processor (Region 1) encountered an error while executing", ex);
                }
            }
            // Perform a final flush to send all remaining events in a batch.
            await eventHubOutput.FlushAsync();
        }

        /// <summary>
        /// The CarEventProcessor function is triggered by a Cosmos DB change feed as
        /// documents are written to the telemetry collection. This function performs
        /// some minor processing of that data by adding a region based on the city,
        /// then sends the enriched data to Event Hubs in batches for further processing.
        /// 
        /// It is configured to read from just one region to demonstrate how to read data
        /// from a specific Cosmos DB region.
        /// </summary>
        /// <param name="input">The Cosmos DB documents sent by the change feed.</param>
        /// <param name="eventHubOutput">The collection of Event Hubs events to send with
        /// enriched car telemetry data.</param>
        /// <param name="log">The logger used for outputting information and errors to the
        /// functions log.</param>
        /// <returns></returns>
        [FunctionName("CarEventProcessorRegion2")]
        public static async Task CarEventProcessorRegion2([CosmosDBTrigger(
            databaseName: "ContosoAuto",
            collectionName: "telemetry",
            // Uses the "Region2" application configuration value to pass in the region 2 name:
            PreferredLocations = "%Region2%",
            ConnectionStringSetting = "CosmosDbConnectionString",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            [EventHub("telemetry",
                Connection="EventHubsConnectionString")]IAsyncCollector<EventData> eventHubOutput,
            ILogger log)
        {
            log.LogInformation($"Cosmos DB processor received {input.Count} documents from Region 2");
            var telemetryProcessing = new TelemetryProcessing();
            if (_cityRegionMap == null) _cityRegionMap = telemetryProcessing.GetCityRegionMap();

            // Parse the incoming messages and get the Region from the City.
            // Default value is blank. Blank regions can be fixed up later in a downstream process.
            // Route data to Event Hubs.
            foreach (var carData in input)
            {
                try
                {
                    if (carData != null)
                    {
                        if (!IsTelemetryData(carData)) continue; // Skip if this document is not telemetry data.
                        var carEventData = await carData.ReadAsAsync<CarEvent>();
                        await telemetryProcessing.ProcessEvent(carEventData,
                            _cityRegionMap, eventHubOutput);
                    }
                }
                catch (Exception ex)
                {
                    log.LogError("Cosmos DB processor (Region 1) encountered an error while executing", ex);
                }
            }
            // Perform a final flush to send all remaining events in a batch.
            await eventHubOutput.FlushAsync();
        }

        private static bool IsTelemetryData(Document carData)
        {
            return carData.GetPropertyValue<string>("collectionType").ToLower() == "telemetry";
        }
    }
}
