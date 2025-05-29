using System;

namespace RuntimeErrorSage.Application.Models.Metrics
{
    public class MetricStats
    {
        public double Sum { get; set; }
        public int Count { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Average { get; set; }
    }
} 
