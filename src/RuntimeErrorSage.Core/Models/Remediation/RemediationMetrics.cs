using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents metrics collected during a remediation operation.
    /// </summary>
    public class RemediationMetrics
    {
        /// <summary>
        /// Gets or sets the unique identifier for this metrics collection.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the execution ID associated with these metrics.
        /// </summary>
        public string ExecutionId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when these metrics were collected.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the resource usage at the start of the remediation.
        /// </summary>
        public ResourceUsage StartResourceUsage { get; set; } = new ResourceUsage();

        /// <summary>
        /// Gets or sets the resource usage at the end of the remediation.
        /// </summary>
        public ResourceUsage EndResourceUsage { get; set; } = new ResourceUsage();

        /// <summary>
        /// Gets or sets the number of retries performed during the remediation.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the total duration of the remediation in milliseconds.
        /// </summary>
        public double DurationMs { get; set; }

        /// <summary>
        /// Gets or sets additional values associated with these metrics.
        /// </summary>
        public Dictionary<string, object> Values { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets labels associated with these metrics.
        /// </summary>
        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();

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

    /// <summary>
    /// Represents resource usage metrics.
    /// </summary>
    public class ResourceUsage
    {
        /// <summary>
        /// Gets or sets the CPU usage percentage.
        /// </summary>
        public double CpuUsage { get; set; }

        /// <summary>
        /// Gets or sets the memory usage in bytes.
        /// </summary>
        public double MemoryUsage { get; set; }

        /// <summary>
        /// Gets or sets the disk usage in bytes.
        /// </summary>
        public double DiskUsage { get; set; }

        /// <summary>
        /// Gets or sets the network usage in bytes.
        /// </summary>
        public double NetworkUsage { get; set; }
    }
} 