using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using CustomSkillFunctions.Models;
using System.Linq;
using System.Collections.Generic;

namespace CustomSkillFunctions
{
    public static class DetectAnomaliesFunction
    {
        // TODO: Replace the service endpoint with the endpoint for your Anomaly Detector service.
        private static readonly string serviceEndpoint = "<enter your service endpoint here>";
        // TODO: Replace the key with a valid service key.
        private static readonly string key = "<enter your api key here>";

        [FunctionName("DetectAnomalies")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("DetectAnomalies function received a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<DetectAnomalyRequest>(requestBody);

            log.LogInformation($"DetectAnomalies function received body: {requestBody}.");

            log.LogInformation($"DetectAnomalies function received body: {requestBody}.");

            if (request?.Values == null)
            {
                return new BadRequestObjectResult("Could not find values array");
            }
            if (request.Values.Any() == false)
            {
                // It could not find a record, then return empty values array.
                return new BadRequestObjectResult("Could not find valid records in values array");
            }

            var document = request.Values.First();
            if (document.RecordId == null)
            {
                return new BadRequestObjectResult("RecordId cannot be null");
            }
            log.LogInformation($"DetectAnomalies function processing record {document.RecordId}.");

            var result = await Detect(document.Data);

            // Put together response.
            var responseRecord = new WebApiResponseRecord
            {
                RecordId = document.RecordId
            };
            var dataRecords = new Dictionary<string, object>
            {
                { "anomalyResult", result },
                { "isAnomaly", result.IsAnomaly },
                { "isPositiveAnomaly", result.IsPositiveAnomaly },
                { "isNegativeAnomaly", result.IsNegativeAnomaly },
                { "expectedValue", result.ExpectedValue },
                { "upperMargin", result.UpperMargin },
                { "lowerMargin", result.LowerMargin }
            };
            
            responseRecord.Data = dataRecords;

            var response = new WebApiEnricherResponse
            {
                Values = new List<WebApiResponseRecord> { responseRecord }
            };

            log.LogInformation($"Response for {document.RecordId } is: {JsonConvert.SerializeObject(response)}");

            return new OkObjectResult(response);
        }

        private static async Task<AnomalyDetectionResponse> Detect(DetectAnomalyData data)
        {
            var anomalyDetectorTrainUri = $"{serviceEndpoint}anomalydetector/v1.0/timeseries/last/detect";
            var timeSeriesData = new Series
            {
                maxAnomalyRatio = 0.25F,
                sensitivity = 95,
                granularity = "minutely"
            };

            // Create a bogus series of engine temps, that are within bounds
            var models = new List<AnomalyModel>();
            var date = DateTime.Now.AddDays(-2);
            var random = new Random(45);

            for (int i = 0; i < 5000; i++)
            {
                models.Add(new AnomalyModel { timestamp = date.AddMinutes(i), value = random.Next(250, 350) });
            }

            // Add in the data from the search record.
            models.Add(new AnomalyModel
            {
                timestamp = date.AddMinutes(5000),
                value = data.EngineTemperature
            });

            timeSeriesData.series = models;

            var requestBody = JsonConvert.SerializeObject(timeSeriesData);

            // Send the training request to the Anomaly Detector batch detect enpoint.
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(anomalyDetectorTrainUri);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", key);

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var responseBody = await response.Content.ReadAsStringAsync();


                    return JsonConvert.DeserializeObject<AnomalyDetectionResponse>(responseBody);
                }
            }
        }
    }
}