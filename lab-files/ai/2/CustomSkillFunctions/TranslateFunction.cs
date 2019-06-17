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
using System.Collections.Generic;
using CustomSkillFunctions.Models;
using System.Linq;

namespace CustomSkillFunctions
{
    public static class TranslateFunction
    {
        static readonly string _path = "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0";

        // NOTE: Replace this example key with a valid subscription key.
        static readonly string _subscriptionKey = "e8c64ad67cb740079fd41ee6e0ddf35a"; //" < enter your api key here>";

        [FunctionName("Translate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            log.LogInformation($"Translate function receivied a request. Request body: {requestBody}");

            var request = JsonConvert.DeserializeObject<TranslateRequest>(requestBody);

            // Validation
            if (request?.Values == null)
            {
                return new BadRequestObjectResult("Could not find values array");
            }
            if (request.Values.Any() == false || string.IsNullOrWhiteSpace(request.Values.First().Data?.Text) == true)
            {
                // It could not find a record, then return empty values array.
                return new BadRequestObjectResult("Could not find valid records in values array");
            }

            var valueToTranslate = request.Values.First();
            if (valueToTranslate.RecordId == null)
            {
                return new BadRequestObjectResult("recordId cannot be null");
            }

            log.LogInformation($"Translate function translating '{valueToTranslate.Data.Text}' from {valueToTranslate.Data.Language} to English.");

            var translatedText = valueToTranslate.Data.Language != "en"
                ? await TranslateText(valueToTranslate.Data.Text)
                : valueToTranslate.Data.Text;

            log.LogInformation($"Translate function translation '{translatedText}'.");

            // Put together response.
            var responseRecord = new WebApiResponseRecord
            {
                Data = new Dictionary<string, object>
                {
                    { "text", translatedText }
                },
                RecordId = valueToTranslate.RecordId
            };

            var response = new WebApiEnricherResponse
            {
                Values = new List<WebApiResponseRecord> { responseRecord }
            };

            log.LogInformation($"Translate function output '{responseRecord}'.");

            return new OkObjectResult(response);
        }

        /// <summary>
        /// Use Cognitive Service to translate text from one language to another.
        /// </summary>
        /// <param name="originalText">The text to translate.</param>
        /// <param name="toLanguage">The language you want to translate to. Default is en (English).</param>
        /// <returns>Asynchronous task that returns the translated text. </returns>
        private static async Task<string> TranslateText(string originalText, string toLanguage = "en")
        {
            var body = new object[] { new { Text = originalText } };
            var requestBody = JsonConvert.SerializeObject(body);

            var uri = $"{_path}&to={toLanguage}";

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(uri);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var deserializedResponseBody = JsonConvert.DeserializeObject <List<TranslationResult>>(responseBody);

                    return deserializedResponseBody.First().Translations.First().Text;
                }
            }
        }
    }
}