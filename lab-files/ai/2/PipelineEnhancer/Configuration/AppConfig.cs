namespace PipelineEnhancer.Configuration
{
    public class AppConfig
    {
        public AnomalyDetectorConfig AnomalyDetector { get; set; }
        public BlobStorageConfig BlobStorage { get; set; }
        public CognitiveServicesConfig CognitiveServices { get; set; }
        public CosmosDbConfig CosmosDb { get; set; }
        public FormRecognizerConfig FormRecognizer { get; set; }
        public FunctionAppConfig FunctionApp { get; set; }
        public PersonalizerConfig Personalizer { get; set; }
        public SearchConfig Search { get; set; }
    }
}