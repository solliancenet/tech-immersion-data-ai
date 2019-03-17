using Newtonsoft.Json;
using System.Collections.Generic;

namespace Search.Common.Models
{
    /// <summary>
    /// Abstract base class for skills. https://docs.microsoft.com/azure/search/cognitive-search-predefined-skills
    /// </summary>
    public class Skill
    {
        /// <summary>
        /// Initializes a new instance of the Skill class.
        /// </summary>
        public Skill()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Skill class.
        /// </summary>
        /// <param name="description">The description of the skill which describes the inputs, outputs, and usage of the skill.</param>
        /// <param name="context">Represents the level at which operations take place, such as the document root or document content (for example, /document or /document/content).</param>
        /// <param name="inputs">Inputs of the skills could be a column in the source data set, or the output of an upstream skill.</param>
        /// <param name="outputs">The output of a skill is either a field in an Azure Search index, or a value that can be consumed as an input by another skill.</param>
        public Skill(string description, string context, IList<InputFieldMappingEntry> inputs, IList<OutputFieldMappingEntry> outputs)
        {
            Description = description;
            Context = context;
            Inputs = inputs;
            Outputs = outputs;
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
        [JsonProperty(PropertyName = "context")]
        public string Context { get; set; } = "/document";

        /// <summary>
        /// Gets or sets inputs of the skills could be a column in the source data set, or
        /// the output of an upstream skill.
        /// </summary>
        [JsonProperty(PropertyName = "inputs")]
        public IList<InputFieldMappingEntry> Inputs { get; set; }

        /// <summary>
        /// Gets or sets the output of a skill is either a field in an Azure Search index,
        /// or a value that can be consumed as an input by another skill.
        /// </summary>
        [JsonProperty(PropertyName = "outputs")]
        public IList<OutputFieldMappingEntry> Outputs { get; set; }

    }
}