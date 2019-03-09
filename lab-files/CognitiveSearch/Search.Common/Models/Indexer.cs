using Newtonsoft.Json;
using System.Collections.Generic;

namespace Search.Common.Models
{
    /// <summary>
    /// Represents an Azure Search indexer. https://docs.microsoft.com/rest/api/searchservice/Indexer-operations
    /// </summary>
    public class Indexer : INamed
    {
        /// <summary>
        /// Initializes a new instance of the Indexer class.
        /// </summary>
        public Indexer()
        {
            FieldMappings = new List<FieldMapping>();
        }

        /// <summary>
        /// Gets or sets the name of the indexer.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the indexer.
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the datasource from which this indexer reads data.
        /// </summary>
        [JsonProperty(PropertyName = "dataSourceName")]
        public string DataSourceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the cognitive skillset executing with this indexer.
        /// </summary>
        [JsonProperty(PropertyName = "skillsetName")]
        public string SkillsetName { get; set; }

        /// <summary>
        /// Gets or sets the name of the index to which this indexer writes data.
        /// </summary>
        [JsonProperty(PropertyName = "targetIndexName")]
        public string TargetIndexName { get; set; }

        /// <summary>
        /// Gets or sets the schedule for this indexer.
        /// </summary>
        [JsonProperty(PropertyName = "schedule")]
        public IndexingSchedule Schedule { get; set; }

        /// <summary>
        /// Gets or sets parameters for indexer execution.
        /// </summary>
        [JsonProperty(PropertyName = "parameters")]
        public IndexingParameters Parameters { get; set; }

        /// <summary>
        /// Gets or sets defines mappings between fields in the data source and corresponding target fields in the index.
        /// </summary>
        [JsonProperty(PropertyName = "fieldMappings")]
        public IList<FieldMapping> FieldMappings { get; set; }

        /// <summary>
        /// Gets or sets output field mappings are applied after enrichment and immediately before indexing.
        /// </summary>
        [JsonProperty(PropertyName = "outputFieldMappings")]
        public IList<FieldMapping> OutputFieldMappings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the indexer is disabled. Default is false.
        /// </summary>
        [JsonProperty(PropertyName = "disabled")]
        public bool? IsDisabled { get; set; }
    }
}