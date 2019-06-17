namespace PipelineEnhancer.Configuration
{
    public class BlobStorageConfig
    {
        public string AccountName { get; set; }
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string SasToken { get; set; }
    }
}