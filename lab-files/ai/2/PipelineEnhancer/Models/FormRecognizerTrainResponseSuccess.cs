using Newtonsoft.Json;
using System.Collections.Generic;

namespace PipelineEnhancer.Models
{
    public class FormRecognizerTrainSuccessResponse
    {
        [JsonProperty("modelId")]
        public string ModelId { get; set; }
        [JsonProperty("trainingDocuments")]
        public List<FormRecognizerTrainingDocument> TrainingDocuments { get; set; }
        [JsonProperty("errors")]
        public List<string> Errors { get; set; }
    }

    public class FormRecognizerTrainingDocument
    {
        [JsonProperty("documentName")]
        public string DocumentName { get; set; }
        [JsonProperty("pages")]
        public int Pages { get; set; }
        [JsonProperty("errors")]
        public List<string> Errors { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class FormRecognizerErrorResponse
    {
        [JsonProperty("error")]
        public FormRecognizerError Error { get; set; }
    }

    public class FormRecognizerError
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        public InnerError InnerError { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class InnerError
    {
        [JsonProperty("requestId")]
        public string RequestId { get; set; }
    }
}
