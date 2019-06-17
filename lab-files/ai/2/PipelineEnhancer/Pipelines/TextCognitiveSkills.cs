using Microsoft.Azure.Search.Models;
using PipelineEnhancer.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PipelineEnhancer.Pipelines
{
    public class TextCognitiveSkills
    {
        #region Index

        public static async Task<Index> GetBaseIndex(string name)
        {
            var analyzerName = AnalyzerName.StandardLucene;
            return await Task.FromResult(new Index
            {
                Name = name,
                Fields = new List<Field>
                {
                    new Field("created_at", DataType.DateTimeOffset)
                    {
                        IsFilterable = true,
                        IsRetrievable = true
                    },
                    new Field("id_str", analyzerName),
                    new Field("id", analyzerName),
                    new Field("text", analyzerName),
                    new Field("rid", analyzerName) { IsKey = true },
                    new Field("people", DataType.Collection(DataType.String), analyzerName),
                    new Field("organizations", DataType.Collection(DataType.String), analyzerName),
                    new Field("locations", DataType.Collection(DataType.String), analyzerName),
                    new Field("keyphrases", DataType.Collection(DataType.String), analyzerName),
                    new Field("language", analyzerName),
                    new Field("user", DataType.Complex, new List<Field>
                    {
                        new Field("id", DataType.Int64),
                        new Field("id_str", analyzerName),
                        new Field("name", analyzerName),
                        new Field("screen_name", analyzerName),
                        new Field("location", analyzerName),
                        new Field("url", analyzerName),
                        new Field("description", analyzerName)
                    }),
                    new Field("entities", DataType.Complex, new List<Field>
                    {
                        new Field("hashtags", DataType.Collection(DataType.Complex), new List<Field>
                        {
                            new Field("indicies", analyzerName),
                            new Field("text", analyzerName),
                        }),
                        new Field("user_mentions", DataType.Collection(DataType.Complex), new List<Field>
                        {
                            new Field("id", DataType.Int64),
                            new Field("id_str", analyzerName),
                            new Field("indicies", DataType.Collection(DataType.Int64)),
                            new Field("name", analyzerName),
                            new Field("screen_name", analyzerName),
                        })
                    }),
                    new Field("symbols", DataType.Collection(DataType.String), analyzerName),
                    new Field("urls", DataType.Collection(DataType.String), analyzerName)
                }
            });
        }

        #endregion

        #region Indexer

        public static async Task<Indexer> GetBaseIndexer(SearchConfig config) => new Indexer
        {
            Name = config.IndexerName,
            Description = "Tweet indexer",
            DataSourceName = config.DataSourceName,
            SkillsetName = config.SkillsetName,
            TargetIndexName = config.IndexName,
            Schedule = new IndexingSchedule(new TimeSpan(0, 5, 0)),
            OutputFieldMappings = new List<FieldMapping>
            {
                await CognitiveSearchHelper.CreateFieldMapping("/document/people", "people"),
                await CognitiveSearchHelper.CreateFieldMapping("/document/organizations", "organizations"),
                await CognitiveSearchHelper.CreateFieldMapping("/document/locations", "locations"),
                await CognitiveSearchHelper.CreateFieldMapping("/document/keyphrases", "keyphrases"),
                await CognitiveSearchHelper.CreateFieldMapping("/document/language", "language")
            }
        };

        #endregion

        #region Skillset

        public static async Task<Skillset> GetBaseSkillset(string name, CognitiveServicesConfig cognitiveServicesConfig)
        {
            return await Task.FromResult(new Skillset
            {
                Name = name,
                Description = "Cognitive skills collection",
                CognitiveServices = new CognitiveServicesByKey(cognitiveServicesConfig.Key, cognitiveServicesConfig.ResourceId),
                Skills = new List<Skill>
                {
                    new EntityRecognitionSkill
                    {
                        Description = "Entity recognition skill",
                        Context = "/document",
                        Categories = new List<EntityCategory>
                        {
                            EntityCategory.Person,
                            EntityCategory.Quantity,
                            EntityCategory.Organization,
                            EntityCategory.Location,
                            EntityCategory.Datetime,
                            EntityCategory.Url,
                            EntityCategory.Email
                        },
                        DefaultLanguageCode = "en",
                        Inputs = new List<InputFieldMappingEntry>
                        {
                            new InputFieldMappingEntry("text", "/document/text")
                        },
                        Outputs = new List<OutputFieldMappingEntry> {
                            new OutputFieldMappingEntry("persons", "people"),
                            new OutputFieldMappingEntry("organizations", "organizations"),
                            new OutputFieldMappingEntry("locations", "locations")
                        }
                    },
                    new KeyPhraseExtractionSkill
                    {
                        Context = "/document",
                        Description = "Key phrase extraction skill",
                        DefaultLanguageCode = "en",
                        Inputs = new List<InputFieldMappingEntry>
                        {
                            new InputFieldMappingEntry("text", "/document/text")
                        },
                        Outputs = new List<OutputFieldMappingEntry> {
                            new OutputFieldMappingEntry("keyPhrases", "keyphrases")
                        }
                    },
                    new LanguageDetectionSkill
                    {
                        Context = "/document",
                        Description = "Language detection skill",
                        Inputs = new List<InputFieldMappingEntry>
                        {
                            new InputFieldMappingEntry("text", "/document/text")
                        },
                        Outputs = new List<OutputFieldMappingEntry> {
                            new OutputFieldMappingEntry("languageCode", "language")
                        }
                    }
                }
            });
        }

        #endregion
    }
}