using Newtonsoft.Json;
using System.Collections.Generic;

namespace Search.Common.Models
{
    public class KeyPhraseExtractionSkill : Skill
    {
        /// <summary>
        /// Initializes a new instance of the KeyPhraseExtractionSkill class.
        /// </summary>
        public KeyPhraseExtractionSkill()
        {
            ODataType = "#Microsoft.Skills.Text.KeyPhraseExtractionSkill";
        }

        /// <summary>
        /// Initializes a new instance of the KeyPhraseExtractionSkill class.
        /// </summary>
        /// <param name="description">The description of the skill which describes the inputs, outputs, and usage of the skill.</param>
        /// <param name="context">Represents the level at which operations take place, such as the document root or document content (for example, /document or /document/content).</param>
        /// <param name="inputs">Inputs of the skills could be a column in the source data set, or the output of an upstream skill.</param>
        /// <param name="outputs">The output of a skill is either a field in an Azure Search index, or a value that can be consumed as an input by another skill.</param>
        /// <param name="defaultLanguageCode">A value indicating which language code to use. Default is en.</param>
        /// <param name="maxKeyPhraseCount">Maximum number of key phrases to return.</param>
        public KeyPhraseExtractionSkill(string description, string context, IList<InputFieldMappingEntry> inputs, IList<OutputFieldMappingEntry> outputs, string defaultLanguageCode = "en", int? maxKeyPhraseCount = null)
        {
            ODataType = "#Microsoft.Skills.Text.KeyPhraseExtractionSkill";
            Context = context;
            Description = description;
            Inputs = inputs;
            Outputs = outputs;
            DefaultLanguageCode = defaultLanguageCode;
            MaxKeyPhraseCount = maxKeyPhraseCount;
        }

        /// <summary>
        /// Gets or sets a value indicating which language code to use. Default is en.
        /// </summary>
        [JsonProperty(PropertyName = "defaultLanguageCode")]
        public string DefaultLanguageCode { get; set; } = "en";

        /// <summary>
        /// Gets or sets a maximum number of key phrases to return.
        /// </summary>
        [JsonProperty(PropertyName = "maxKeyPhraseCount")]
        public int? MaxKeyPhraseCount { get; set; }
    }
}