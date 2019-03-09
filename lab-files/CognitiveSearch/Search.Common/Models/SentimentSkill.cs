using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Search.Common.Models
{
    public class SentimentSkill : Skill
    {
        /// <summary>
        /// Initializes a new instance of the SentimentSkill class.
        /// </summary>
        public SentimentSkill()
        {
            ODataType = "#Microsoft.Skills.Text.SentimentSkill";
        }

        /// <summary>
        /// Initializes a new instance of the SentimentSkill class.
        /// </summary>
        /// <param name="description">The description of the skill which describes the inputs, outputs, and usage of the skill.</param>
        /// <param name="context">Represents the level at which operations take place, such as the document root or document content (for example, /document or /document/content).</param>
        /// <param name="inputs">Inputs of the skills could be a column in the source data set, or the output of an upstream skill.</param>
        /// <param name="outputs">The output of a skill is either a field in an Azure Search index, or a value that can be consumed as an input by another skill.</param>
        /// <param name="defaultLanguageCode">A value indicating which language code to use. Default is en.</param>
        public SentimentSkill(string description, string context, IList<InputFieldMappingEntry> inputs, IList<OutputFieldMappingEntry> outputs, string defaultLanguageCode = "en")
        {
            ODataType = "#Microsoft.Skills.Text.SentimentSkill";
            Context = context;
            Description = description;
            Inputs = inputs;
            Outputs = outputs;
            DefaultLanguageCode = defaultLanguageCode;
        }

        /// <summary>
        /// Gets or sets a value indicating which language code to use. Default is en.
        /// </summary>
        [JsonProperty(PropertyName = "defaultLanguageCode")]
        public string DefaultLanguageCode { get; set; } = "en";
    }
}