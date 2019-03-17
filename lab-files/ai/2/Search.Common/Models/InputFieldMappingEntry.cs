using Newtonsoft.Json;

namespace Search.Common.Models
{
    /// <summary>
    /// Input field mapping for a skill.
    /// </summary>
    public class InputFieldMappingEntry
    {
        /// <summary>
        /// Initializes a new instance of the InputFieldMappingEntry class.
        /// </summary>
        /// <param name="name">The name of the input.</param>
        /// <param name="source">The source of the input.</param>
        public InputFieldMappingEntry(string name, string source)
        {
            Name = name;
            Source = source;
        }

        /// <summary>
        /// Gets or sets the name of the input.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the source of the input.
        /// </summary>
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
    }
}