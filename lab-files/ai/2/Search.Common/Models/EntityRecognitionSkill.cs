using Newtonsoft.Json;
using System.Collections.Generic;

namespace Search.Common.Models
{
    public class EntityRecognitionSkill : Skill
    {
        /// <summary>
        /// Initializes a new instance of the EntityRecognitionSkill class.
        /// </summary>
        public EntityRecognitionSkill()
        {
            ODataType = "#Microsoft.Skills.Text.EntityRecognitionSkill";
        }

        /// <summary>
        /// Gets or sets a list of named entity categories.
        /// </summary>
        [JsonProperty(PropertyName = "categories")]
        public IList<string> Categories { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating which language code to use. Default is en.
        /// </summary>
        [JsonProperty(PropertyName = "defaultLanguageCode")]
        public string DefaultLanguageCode { get; set; } = "en";

        /// <summary>
        /// Gets or sets a value between 0 and 1 to indicate the confidence of the results.
        /// </summary>
        [JsonProperty(PropertyName = "minimumPrecision")]
        public double? MinimumPrecision { get; set; }
    }
}