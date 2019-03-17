using Newtonsoft.Json;
using System.Collections.Generic;

namespace Search.Common.Models
{
    /// <summary>
    /// A list of cognitive skills. https://docs.microsoft.com/azure/search/cognitive-search-tutorial-blob
    /// </summary>
    public class Skillset : INamed
    {
        /// <summary>
        /// Initializes a new instance of the Skillset class.
        /// </summary>
        public Skillset()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Skillset class.
        /// </summary>
        /// <param name="name">The name of the skillset.</param>
        /// <param name="description">The description of the skillset.</param>
        /// <param name="skills">A list of skills in the skillset.</param>
        /// <param name="eTag">The ETag of the skillset.</param>
        public Skillset(string name, string description, IList<Skill> skills) //, string eTag = null)
        {
            Name = name;
            Description = description;
            Skills = skills;
            //ETag = eTag;
        }

        /// <summary>
        /// Gets or sets the name of the skillset.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the skillset.
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a list of skills in the skillset.
        /// </summary>
        [JsonProperty(PropertyName = "skills")]
        public IList<Skill> Skills { get; set; }

        ///// <summary>
        ///// Gets or sets the ETag of the skillset.
        ///// </summary>
        //[JsonProperty(PropertyName = "@odata.etag")]
        //public string ETag { get; set; }

        /// <summary>
        /// Defines the connection to a Cognitive Services account.
        /// </summary>
        [JsonProperty(PropertyName = "cognitiveServices")]
        public CognitiveServices CognitiveServices { get; set; }
    }
}