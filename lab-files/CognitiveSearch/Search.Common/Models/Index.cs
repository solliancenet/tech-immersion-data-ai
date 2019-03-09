using Newtonsoft.Json;
using System.Collections.Generic;

namespace Search.Common.Models
{
    /// <summary>
    /// Represents an index definition in Azure Search, which describes the fields and
    /// search behavior of an index.
    /// </summary>
    public class Index : INamed
    {
        /// <summary>
        /// Initializes a new instance of the Index class.
        /// </summary>
        public Index()
        {
        }

        /// <summary>
        /// Gets or sets the name of the index.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the fields of the index.
        /// </summary>
        [JsonProperty(PropertyName = "fields")]
        public IList<Field> Fields { get; set; }
    }
}