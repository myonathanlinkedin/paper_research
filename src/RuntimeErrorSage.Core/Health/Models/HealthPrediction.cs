using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Health.Models
{
    public class HealthPrediction
    {
        public double PredictedHealthScore { get; set; }
        public TimeSpan? TimeToUnhealthy { get; set; }
        public double Confidence { get; set; }
        public Dictionary<string, MetricTrend> Trends { get; set; } = new();
    }
} 
