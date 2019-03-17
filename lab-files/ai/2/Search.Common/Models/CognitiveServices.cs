using Newtonsoft.Json;

namespace Search.Common.Models
{
    public class CognitiveServices
    {
        public CognitiveServices(string description, string key)
        {
            ODataType = "#Microsoft.Azure.Search.CognitiveServicesByKey";
            Description = description;
            Key = key;
        }

        /// <summary>
        /// Gets or sets the @odata.type of the skill.
        /// </summary>
        [JsonProperty(PropertyName = "@odata.type")]
        public string ODataType { get; set; }

        /// <summary>
        /// Gets or sets the description of the skill which describes the inputs, outputs, and usage of the skill.
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets represents the level at which operations take place, such as the
        /// document root or document content(for example, /document or /document/content).
        /// Default is /document.
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
    }
}
