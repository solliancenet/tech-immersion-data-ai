using System.Collections.Generic;

namespace Search.Common.Models
{
    public class LanguageDetectionSkill : Skill
    {
        /// <summary>
        /// Initializes a new instance of the LanguageDetectionSkill class.
        /// </summary>
        public LanguageDetectionSkill()
        {
            ODataType = "#Microsoft.Skills.Text.LanguageDetectionSkill";
        }

        /// <summary>
        /// Initializes a new instance of the LanguageDetectionSkill class.
        /// </summary>
        /// <param name="description">The description of the skill which describes the inputs, outputs, and usage of the skill.</param>
        /// <param name="context">Represents the level at which operations take place, such as the document root or document content (for example, /document or /document/content).</param>
        /// <param name="inputs">Inputs of the skills could be a column in the source data set, or the output of an upstream skill.</param>
        /// <param name="outputs">The output of a skill is either a field in an Azure Search index, or a value that can be consumed as an input by another skill.</param>
        /// <param name="categories">A list of named entity categories.</param>
        /// <param name="defaultLanguageCode">A value indicating which language code to use. Default is en.</param>
        /// <param name="minimumPrecision">A value between 0 and 1 to indicate the confidence of the results.</param>
        public LanguageDetectionSkill(string description, string context, IList<InputFieldMappingEntry> inputs, IList<OutputFieldMappingEntry> outputs)
        {
            ODataType = "#Microsoft.Skills.Text.LanguageDetectionSkill";
            Context = context;
            Description = description;
            Inputs = inputs;
            Outputs = outputs;
        }
    }
}