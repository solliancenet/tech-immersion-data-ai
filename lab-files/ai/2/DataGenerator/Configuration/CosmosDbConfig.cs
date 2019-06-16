namespace DataGenerator.Configuration
{
    public class CosmosDbConfig
    {
        public string ConnectionString { get; set; }
        public string SecondsToRun { get; set; }
        public int MillisecondsToRun => int.Parse(SecondsToRun) * 1000;
        public string DatabaseId { get; set; }
        public string TweetsContainerId { get; set; }
        public string TweetsPartitionKey { get; set; }
        public string TelemetryContainerId { get; set; }
        public string TelemetryPartitionKey { get; set; }
    }
}
