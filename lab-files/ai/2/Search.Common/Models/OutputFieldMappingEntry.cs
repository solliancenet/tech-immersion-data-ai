using Newtonsoft.Json;

namespace Search.Common.Models
{
    /// <summary>
    /// Output field mapping for a skill. https://docs.microsoft.com/rest/api/searchservice/naming-rules
    /// </summary>
    public class OutputFieldMappingEntry
    {
        /// <summary>
        /// Initializes a new instance of the OutputFieldMappingEntry class.
        /// </summary>
        /// <param name="name">The name of the output defined by the skill.</param>
        /// <param name="targetName">The target name of the output. It is optional and default to name.</param>
        public OutputFieldMappingEntry(string name, string targetName = null)
        {
            Name = name;
            TargetName = targetName;
        }

        /// <summary>
        /// Gets or sets the name of the output defined by the skill.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the target name of the output. It is optional and default to name.
        /// </summary>
        [JsonProperty(PropertyName = "targetName")]
        public string TargetName { get; set; }
    }
}