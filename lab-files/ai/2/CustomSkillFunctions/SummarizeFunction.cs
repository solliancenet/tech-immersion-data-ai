using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CustomSkillFunctions.Models;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;

namespace CustomSkillFunctions
{
    public static class SummarizeFunction
    {
        static readonly string _path = "https://"; // TODO: Add the path for your deployed machine learning model.

        [FunctionName("Summarize")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation($"Summarize function receivied a request. Request body: {requestBody}");

            var request = JsonConvert.DeserializeObject<CustomSkillRequest>(requestBody);

            // Validation
            if (request?.Values == null)
            {
                return new BadRequestObjectResult("Could not find values array");
            }
            if (request.Values.Any() == false || request.Values.First().Data == null)
            {
                // It could not find a record, then return empty values array.
                return new BadRequestObjectResult("Could not find valid records in values array");
            }

            var valueToSummarize = request.Values.First();
            if (valueToSummarize.RecordId == null)
            {
                return new BadRequestObjectResult("recordId cannot be null");
            }

            string textToSummarize = valueToSummarize.Data.text;

            if (string.IsNullOrWhiteSpace(textToSummarize))
            {
                return new BadRequestObjectResult("Text to summarize is required.");
            }

            log.LogInformation($"Summarize function creating summary text for '{textToSummarize}'.");

            var summaryText = await SummarizeText(textToSummarize);

            log.LogInformation($"Summarize function summary: '{summaryText}'.");

            // Put together response.
            var responseRecord = new WebApiResponseRecord
            {
                Data = new Dictionary<string, object>
                {
                    { "summaryText", summaryText }
                },
                RecordId = valueToSummarize.RecordId
            };

            var response = new WebApiEnricherResponse
            {
                Values = new List<WebApiResponseRecord> { responseRecord }
            };

            log.LogInformation($"Summary function output '{responseRecord}'.");

            return new OkObjectResult(response);
        }

        private static async Task<string> SummarizeText(string textToSummarize)
        {
            var body = new object[] { new { Text = textToSummarize } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    //request.Method = HttpMethod.Post;
                    //request.RequestUri = new Uri(_path);
                    //request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    // TODO: Add appropriate headers for calling a deployed ML model...
                    //request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

                    //var response = await client.SendAsync(request).ConfigureAwait(false);
                    //var responseBody = await response.Content.ReadAsStringAsync();
                    //var deserializedResponseBody = JsonConvert.DeserializeObject<List<TranslationResult>>(responseBody);

                    //return deserializedResponseBody.First().Summarizations.First().Text;
                    return "your text summary...";
                }
            }
        }
    }
}
