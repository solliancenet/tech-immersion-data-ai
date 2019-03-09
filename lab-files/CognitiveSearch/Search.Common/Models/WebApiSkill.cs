using Newtonsoft.Json;
using System.Collections.Generic;

namespace Search.Common.Models
{
    public class WebApiSkill : Skill
    {
        /// <summary>
        /// Initializes a new instance of the WebApiSkill class.
        /// </summary>
        public WebApiSkill()
        {
            ODataType = "#Microsoft.Skills.Custom.WebApiSkill";
            Description = "Custom skill";
            BatchSize = 1;
        }

        /// <summary>
        /// Initializes a new instance of the WebApiSkill class.
        /// </summary>
        /// <param name="description">The description of the skill which describes the inputs, outputs, and usage of the skill.</param>
        /// <param name="context">Represents the level at which operations take place, such as the document root or document content (for example, /document or /document/content). Default is /document.</param>
        /// <param name="inputs">Inputs of the skills could be a column in the source data set, or the output of an upstream skill.</param>
        /// <param name="outputs">The output of a skill is either a field in an Azure Search index, or a value that can be consumed as an input by another skill.</param>
        /// <param name="uri">URL to the Web API endpoint.</param>
        /// <param name="batchSize">Batch size to use.</param>
        /// <param name="httpMethod">HTTP Method to use when calling the API endpoint.</param>
        public WebApiSkill(string description, string context, IList<InputFieldMappingEntry> inputs, IList<OutputFieldMappingEntry> outputs, string uri, int batchSize = 1, string httpMethod = "POST")
        {
            ODataType = "#Microsoft.Skills.Text.EntityRecognitionSkill";
            Context = context;
            Description = description;
            Inputs = inputs;
            Outputs = outputs;
            Uri = uri;
            BatchSize = batchSize;
            HttpMethod = httpMethod;
        }

        /// <summary>
        /// Gets or sets a list of named entity categories.
        /// </summary>
        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets a value indicating which language code to use. Default is en.
        /// </summary>
        [JsonProperty(PropertyName = "batchSize")]
        public int BatchSize { get; set; } = 1;

        /// <summary>
        /// Gets or sets a value between 0 and 1 to indicate the confidence of the results.
        /// </summary>
        [JsonProperty(PropertyName = "httpMethod")]
        public string HttpMethod { get; set; }
    }
}