using Newtonsoft.Json;

namespace Search.Common.Models
{
    /// <summary>
    /// Defines a mapping between a field in a data source and a target field in an index.
    /// https://docs.microsoft.com/azure/search/search-indexer-field-mappings
    /// </summary>
    public class FieldMapping
    {
        /// <summary>
        /// Initializes a new instance of the FieldMapping class.
        /// </summary>
        public FieldMapping()
        {
        }

        /// <summary>
        /// Gets or sets the name of the field in the data source.
        /// </summary>
        [JsonProperty(PropertyName = "sourceFieldName")]
        public string SourceFieldName { get; set; }

        /// <summary>
        /// Gets or sets the name of the target field in the index. Same as the source field
        /// name by default.
        /// </summary>
        [JsonProperty(PropertyName = "targetFieldName")]
        public string TargetFieldName { get; set; }
    }
}