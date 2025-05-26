using System;

namespace RuntimeErrorSage.Core.Models.Metrics
{
    public class MetricValue
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public DateTime Timestamp { get; set; }
        public string Unit { get; set; }
        public Dictionary<string, string> Tags { get; set; }
    }
} 
