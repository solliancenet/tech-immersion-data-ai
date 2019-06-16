using System;
using System.Collections.Generic;

namespace CustomSkillFunctions.Models
{
    public class AnomalyModel
    {
        public DateTime timestamp { get; set; }
        public float value { get; set; }
    }

    public class Series
    {
        public List<AnomalyModel> series { get; set; }
        public float maxAnomalyRatio { get; set; }
        public int sensitivity { get; set; }
        public string granularity { get; set; }
    }
}