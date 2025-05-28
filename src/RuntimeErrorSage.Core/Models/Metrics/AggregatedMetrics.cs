using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Metrics
{
    public class AggregatedMetrics
    {
        public string MetricName { get; set; } = string.Empty;
        public double Sum { get; set; }
        public int Count { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Average { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Dictionary<string, string> Labels { get; set; } = new();
    }
} 
