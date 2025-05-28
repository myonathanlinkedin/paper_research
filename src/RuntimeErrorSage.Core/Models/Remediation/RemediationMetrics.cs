using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    public class RemediationMetrics
    {
        public string RemediationId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan Duration => EndTime.HasValue ? EndTime.Value - StartTime : DateTime.UtcNow - StartTime;
        public int TotalSteps { get; set; }
        public int CompletedSteps { get; set; }
        public int FailedSteps { get; set; }
        public int RolledBackSteps { get; set; }
        public Dictionary<string, MetricValue> Metrics { get; set; }
        public Dictionary<string, object> Metadata { get; set; }

        public RemediationMetrics()
        {
            StartTime = DateTime.UtcNow;
            Metrics = new Dictionary<string, MetricValue>();
            Metadata = new Dictionary<string, object>();
        }

        public RemediationMetrics(string remediationId) : this()
        {
            RemediationId = remediationId;
        }

        public void AddMetric(string name, double value, string unit = null)
        {
            Metrics[name] = new MetricValue(name, value, unit);
        }

        public void AddMetric(MetricValue metric)
        {
            Metrics[metric.Name] = metric;
        }

        public void Complete()
        {
            EndTime = DateTime.UtcNow;
        }

        public void AddMetadata(string key, object value)
        {
            Metadata[key] = value;
        }

        public override string ToString()
        {
            return $"Remediation {RemediationId}: {CompletedSteps}/{TotalSteps} steps completed in {Duration.TotalSeconds:F2}s";
        }
    }
} 