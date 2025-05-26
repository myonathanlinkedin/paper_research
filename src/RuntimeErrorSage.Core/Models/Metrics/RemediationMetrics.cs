using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Models.Execution;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Models.Metrics
{
    /// <summary>
    /// Represents metrics for a remediation operation.
    /// </summary>
    public class RemediationMetrics
    {
        /// <summary>
        /// Gets or sets the unique identifier of the metrics.
        /// </summary>
        public string MetricsId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the remediation identifier.
        /// </summary>
        public string RemediationId { get; set; }

        /// <summary>
        /// Gets or sets the start time of the remediation.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the remediation.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the total duration in milliseconds.
        /// </summary>
        public double TotalDurationMs => EndTime.HasValue ? (EndTime.Value - StartTime).TotalMilliseconds : 0;

        /// <summary>
        /// Gets or sets the number of steps executed.
        /// </summary>
        public int StepsExecuted { get; set; }

        /// <summary>
        /// Gets or sets the number of successful steps.
        /// </summary>
        public int SuccessfulSteps { get; set; }

        /// <summary>
        /// Gets or sets the number of failed steps.
        /// </summary>
        public int FailedSteps { get; set; }

        /// <summary>
        /// Gets or sets the success rate (0-1).
        /// </summary>
        public double SuccessRate => StepsExecuted > 0 ? (double)SuccessfulSteps / StepsExecuted : 0;

        /// <summary>
        /// Gets or sets the failure rate (0-1).
        /// </summary>
        public double FailureRate => StepsExecuted > 0 ? (double)FailedSteps / StepsExecuted : 0;

        /// <summary>
        /// Gets or sets the resource usage metrics.
        /// </summary>
        public ResourceUsage ResourceUsage { get; set; } = new();

        /// <summary>
        /// Gets or sets the step metrics.
        /// </summary>
        public List<StepMetrics> StepMetrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the strategy metrics.
        /// </summary>
        public List<StrategyMetrics> StrategyMetrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the metrics metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// Represents metrics for a remediation step.
    /// </summary>
    public class StepMetrics
    {
        /// <summary>
        /// Gets or sets the unique identifier of the step metrics.
        /// </summary>
        public string StepMetricsId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the step identifier.
        /// </summary>
        public string StepId { get; set; }

        /// <summary>
        /// Gets or sets the step name.
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// Gets or sets the start time of the step.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the step.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration in milliseconds.
        /// </summary>
        public double DurationMs => EndTime.HasValue ? (EndTime.Value - StartTime).TotalMilliseconds : 0;

        /// <summary>
        /// Gets or sets whether the step was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the error message if the step failed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the number of retries.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the resource usage metrics.
        /// </summary>
        public ResourceUsage ResourceUsage { get; set; } = new();

        /// <summary>
        /// Gets or sets the step metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// Represents resource usage metrics.
    /// </summary>
    public class ResourceUsage
    {
        /// <summary>
        /// Gets or sets the CPU usage percentage.
        /// </summary>
        public double CpuUsagePercent { get; set; }

        /// <summary>
        /// Gets or sets the memory usage in bytes.
        /// </summary>
        public long MemoryUsageBytes { get; set; }

        /// <summary>
        /// Gets or sets the disk usage in bytes.
        /// </summary>
        public long DiskUsageBytes { get; set; }

        /// <summary>
        /// Gets or sets the network usage in bytes.
        /// </summary>
        public long NetworkUsageBytes { get; set; }

        /// <summary>
        /// Gets or sets the number of threads used.
        /// </summary>
        public int ThreadCount { get; set; }

        /// <summary>
        /// Gets or sets the number of handles used.
        /// </summary>
        public int HandleCount { get; set; }

        /// <summary>
        /// Gets or sets the resource usage metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// Represents a metric value with timestamp.
    /// </summary>
    public class MetricValue
    {
        /// <summary>
        /// Gets or sets the timestamp of the metric.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the value of the metric.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the metric metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 
