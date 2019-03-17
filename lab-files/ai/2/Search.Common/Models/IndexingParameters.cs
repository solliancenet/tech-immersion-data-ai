using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Search.Common.Models
{
    /// <summary>
    /// Represents parameters for indexer execution.
    /// </summary>
    public class IndexingParameters
    {
        /// <summary>
        /// Initializes a new instance of the IndexingParameters class.
        /// </summary>
        public IndexingParameters() { }

        /// <summary>
        /// Gets or sets the number of items that are read from the data source and indexed
        /// as a single batch in order to improve performance. The default depends on the
        /// data source type.
        /// </summary>
        [JsonProperty(PropertyName = "batchSize")]
        public int? BatchSize { get; set; } = null;

        /// <summary>
        /// Gets or sets the maximum number of items that can fail indexing for indexer execution
        /// to still be considered successful. -1 means no limit. Default is 0.
        /// </summary>
        [JsonProperty(PropertyName = "maxFailedItems")]
        public int? MaxFailedItems { get; set; } = null;

        /// <summary>
        /// Gets or sets the maximum number of items in a single batch that can fail indexing
        /// for the batch to still be considered successful. -1 means no limit. Default is 0.
        /// </summary>
        [JsonProperty(PropertyName = "maxFailedItemsPerBatch")]
        public int? MaxFailedItemsPerBatch { get; set; } = null;

        /// <summary>
        /// Gets or sets whether indexer will base64-encode all values that are inserted
        /// into key field of the target index. This is needed if keys can contain characters
        /// that are invalid in keys (such as dot '.'). Default is false.
        /// </summary>
        [JsonProperty(PropertyName = "base64EncodeKeys")]
        [Obsolete("This property is obsolete. Please create a field mapping using 'FieldMapping.Base64Encode' instead.")]
        public bool? Base64EncodeKeys { get; set; } = false;

        /// <summary>
        /// Gets or sets a dictionary of indexer-specific configuration properties. Each
        /// name is the name of a specific property. Each value must be of a primitive type.
        /// </summary>
        [JsonProperty(PropertyName = "configuration")]
        public IDictionary<string, object> Configuration { get; set; } = new Dictionary<string, object>(new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("assumeOrderByHighWaterMarkColumn", true) });
    }
}