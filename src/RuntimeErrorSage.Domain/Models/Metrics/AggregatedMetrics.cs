using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Metrics
{
    public class AggregatedMetrics
    {
        public string MetricName { get; } = string.Empty;
        public double Sum { get; }
        public int Count { get; }
        public double Min { get; }
        public double Max { get; }
        public double Average { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public Dictionary<string, string> Labels { get; set; } = new();
    }
} 






