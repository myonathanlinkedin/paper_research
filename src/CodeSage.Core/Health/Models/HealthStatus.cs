using System;
using System.Collections.Generic;

namespace CodeSage.Core.Health.Models
{
    public class HealthStatus
    {
        public string ServiceName { get; set; } = string.Empty;
        public bool IsHealthy { get; set; }
        public double HealthScore { get; set; }
        public Dictionary<string, double> Metrics { get; set; } = new();
        public HealthPrediction? Prediction { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, object> Details { get; set; } = new();
    }
} 