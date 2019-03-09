using Newtonsoft.Json;
using System;

namespace Search.Common.Models
{
    /// <summary>
    /// Represents a schedule for indexer execution.
    /// </summary>
    public class IndexingSchedule
    {
        /// <summary>
        ///  Initializes a new instance of the IndexingSchedule class.
        /// </summary>
        /// <param name="interval">The interval of time between indexer executions.</param>
        public IndexingSchedule(string interval = "PT5M")
        {
            Interval = interval;
            StartTime = DateTimeOffset.Now;
        }

        /// <summary>
        /// Gets or sets the interval of time between indexer executions.
        /// </summary>
        [JsonProperty(PropertyName = "interval")]
        public string Interval { get; set; } = "PT5M";

        /// <summary>
        /// Gets or sets the time when an indexer should start running.
        /// </summary>
        [JsonProperty(PropertyName = "startTime")]
        public DateTimeOffset? StartTime { get; set; } = DateTimeOffset.Now;
    }
}