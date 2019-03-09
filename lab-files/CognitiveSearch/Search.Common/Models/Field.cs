using Newtonsoft.Json;

namespace Search.Common.Models
{
    /// <summary>
    /// Represents a field in an index definition in Azure Search, which describes the
    /// name, data type, and search behavior of a field. https://docs.microsoft.com/rest/api/searchservice/Create-Index
    /// </summary>
    public class Field
    {   
        /// <summary>
        /// Initializes a new instance of the Field class.
        /// </summary>
        public Field()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the field can be used in orderby expressions.
        /// Not valid for string collection fields. Default is false.
        /// </summary>
        [JsonProperty("sortable")]
        public bool IsSortable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field can be used in filter expressions.
        /// Default is false.
        /// </summary>
        [JsonProperty("filterable")]
        public bool IsFilterable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field is included in full-text searches.
        /// Valid only for string or string collection fields. Default is false.
        /// </summary>
        [JsonProperty("searchable")]
        public bool IsSearchable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field is the key of the index. Valid
        /// only for string fields. Every index must have exactly one key field.
        /// </summary>
        [JsonProperty("key")]
        public bool IsKey { get; set; }

        /// <summary>
        /// Gets or sets the names of the synonym maps for the field. This option enables
        /// query time synonym expansion for searches against the field and can be used only
        /// on searchable fields.
        /// </summary>
        [JsonProperty(PropertyName = "synonymMaps")]
        public string[] SynonymMaps { get; set; } = new string[0];

        /// <summary>
        /// Gets or sets the name of the analyzer used at search time for the field. This
        /// option can be used only with searchable fields. It must be set together with
        /// IndexAnalyzer and it cannot be set together with the Analyzer option. This analyzer
        /// can be updated on an existing field. https://docs.microsoft.com/rest/api/searchservice/Language-support
        /// </summary>
        [JsonProperty(PropertyName = "searchAnalyzer")]
        public string SearchAnalyzer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is possible to facet on this field.
        /// Not valid for geo-point fields. Default is false.
        /// </summary>
        [JsonProperty("facetable")]
        public bool IsFacetable { get; set; }

        /// <summary>
        /// Gets or sets the name of the analyzer to use for the field at search time and
        /// indexing time. This option can be used only with searchable fields and it can't
        /// be set together with either SearchAnalyzer or IndexAnalyzer. Once the analyzer
        /// is chosen, it cannot be changed for the field. https://docs.microsoft.com/rest/api/searchservice/Language-support
        /// </summary>
        [JsonProperty(PropertyName = "analyzer")]
        public string Analyzer { get; set; }

        /// <summary>
        /// Gets or sets the data type of the field.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the field. https://docs.microsoft.com/rest/api/searchservice/Naming-rules
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the analyzer used at indexing time for the field. This
        /// option can be used only with searchable fields. It must be set together with
        /// SearchAnalyzer and it cannot be set together with the Analyzer option. Once the
        /// analyzer is chosen, it cannot be changed for the field. https://docs.microsoft.com/rest/api/searchservice/Language-support
        /// </summary>
        [JsonProperty(PropertyName = "indexAnalyzer")]
        public string IndexAnalyzer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field can be returned in a search
        /// result. Default is true.
        /// </summary>
        [JsonProperty("retrievable")]
        public bool IsRetrievable { get; set; }
    }
}