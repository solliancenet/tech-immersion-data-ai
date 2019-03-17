using Newtonsoft.Json;
using System.Collections.Generic;

namespace Search.Common.Models
{
    public class ApiCollectionResponse<T> where T: class
    {
        /// <summary>
        /// Gets or sets the name of the skillset.
        /// </summary>
        [JsonProperty(PropertyName = "@odata.context")]
        public string ODataContext { get; set; }

        /// <summary>
        /// Gets or sets the name of the skillset.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public List<T> Value { get; set; }
    }
}