using CustomSkillFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CustomSkillFunctions
{
    public static class AnalyzeFormFunction
    {
        private static readonly string serviceEndpoint = "https://westus2.api.cognitive.microsoft.com/";
        private static readonly string key = "9d1079dd70494ac3b366a8a91e363b5b";

        [FunctionName("AnalyzeForm")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("AnalyzeForm function received a request.");

            string modelId = req.Query["modelId"];
            if(string.IsNullOrWhiteSpace(modelId))
            {
                return new BadRequestObjectResult("The Form Recognizer ModelId must be passed in the query string.");
            }

            log.LogInformation($"AnalyzeForm function using Form Recognizer model {modelId}.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<AnalyzeFormRequest>(requestBody);

            log.LogInformation($"AnalyzeForm function received body: {requestBody}.");

            if (request?.Values == null)
            {
                return new BadRequestObjectResult("Could not find values array");
            }
            if (request.Values.Any() == false || string.IsNullOrWhiteSpace(request.Values.First().Data?.StorageUri) == true)
            {
                // It could not find a record, then return empty values array.
                return new BadRequestObjectResult("Could not find valid records in values array");
            }

            var document = request.Values.First();
            if (document.RecordId == null)
            {
                return new BadRequestObjectResult("RecordId cannot be null");
            }

            log.LogInformation($"AnalyzeForm function processing record {document.RecordId}.");

            log.LogInformation($"AnalyzeForm function is retrieving the document '{document.Data.StorageUri}'.");
            var formBytes = await GetDocumentFromStorage(document.Data);
            log.LogInformation($"AnalyzeForm function analyzing '{document.Data.StorageUri}' using model ID {modelId}.");

            var analyzedForm = await AnalyzeForm(document.Data, modelId, formBytes);

            log.LogInformation($"AnalyzeForm function completed analyzing document '{document.Data.StorageUri}'.");
            log.LogInformation($"Analyzed form result: {JsonConvert.SerializeObject(analyzedForm)}");

            var form = JsonConvert.DeserializeObject<FormRecognizerResponse>(analyzedForm);

            // Put together response.
            var responseRecord = new WebApiResponseRecord
            {
                RecordId = document.RecordId
            };

            var page = form.Pages.First();
            var dataRecords = new Dictionary<string, object>
            {
                { "formHeight", page.Height },
                { "formWidth", page.Width }
            };

            var keyValuePairs = new List<string>();
            foreach(var kvp in page.KeyValuePairs)
            {
                keyValuePairs.Add($"{kvp.Key.First().Text}: {string.Join(" ", kvp.Value.Select(v => v.Text).ToList())}");
            }
            dataRecords.Add("formKeyValuePairs", keyValuePairs);

            var columns = new List<string>();
            foreach (var column in page.Tables.First().Columns)
            {
                columns.Add($"{column.Header.First().Text}: {string.Join(" ", column.Entries.First().Select(v => v.Text).ToList())}");
            }
            dataRecords.Add("formColumns", columns);

            responseRecord.Data = dataRecords;

            var response = new WebApiEnricherResponse
            {
                Values = new List<WebApiResponseRecord> { responseRecord }
            };

            log.LogInformation($"Response for {document.Data.StorageUri} is: {JsonConvert.SerializeObject(response)}");

            return new OkObjectResult(response);
        }

        /// <summary>
        /// Use Cognitive Service to translate text from one language to another.
        /// </summary>
        /// <param name="originalText">The text to translate.</param>
        /// <param name="toLanguage">The language you want to translate to. Default is en (English).</param>
        /// <returns>Asynchronous task that returns the translated text. </returns>
        private static async Task<string> AnalyzeForm(AnalyzeFormData formData, string modelId, byte[] fileBytes)
        {
            var uri = $"{serviceEndpoint}/formrecognizer/v1.0-preview/custom/models/{modelId}/analyze";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    using(var requestContent = new ByteArrayContent(fileBytes))
                    {
                        requestContent.Headers.ContentType = new MediaTypeHeaderValue(formData.ContentType);
                        var response = await client.PostAsync(uri, requestContent);
                        var responseString = response.Content.ReadAsStringAsync();
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }

        private static async Task<byte[]> GetDocumentFromStorage(AnalyzeFormData formData)
        {
            var uri = $"{formData.StorageUri}{formData.StorageSasToken}";

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Get;
                    request.RequestUri = new Uri(uri);

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    return await response.Content.ReadAsByteArrayAsync();
                }
            }
        }
    }
}
