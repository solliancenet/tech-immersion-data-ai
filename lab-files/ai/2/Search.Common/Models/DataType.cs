namespace Search.Common.Models
{
    /// <summary>
    /// Defines the data type of a field in an Azure Search index.
    /// </summary>
    public struct DataType
    {
        public static string String = "Edm.String";
        public static string Double = "Edm.Double";
        public static string StringCollection = "Collection(Edm.String)";
    }
}