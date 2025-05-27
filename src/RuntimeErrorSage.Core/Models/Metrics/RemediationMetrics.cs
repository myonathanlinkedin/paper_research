using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Metrics
{
    /// <summary>
    /// Represents metrics for a remediation operation.
    /// </summary>
    public class RemediationMetrics
    {
        /// <summary>
        /// Gets or sets the execution identifier.
        /// </summary>
        public string ExecutionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the metric values.
        /// </summary>
        public Dictionary<string, object> Values { get; set; } = new();

        /// <summary>
        /// Gets or sets the metric labels.
        /// </summary>
        public Dictionary<string, string> Labels { get; set; } = new();

        /// <summary>
        /// Gets or sets the unique identifier for these metrics.
        /// </summary>
        public required string MetricsId { get; set; }

        /// <summary>
        /// Gets or sets the remediation ID these metrics are for.
        /// </summary>
        public required string RemediationId { get; set; }

        /// <summary>
        /// Gets or sets when the remediation started.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets when the remediation ended.
        /// </summary>
        public DateTime EndTime { get; set; }

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
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the remediation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error message if any.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the remediation status.
        /// </summary>
        public RemediationStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the strategy name.
        /// </summary>
        public string StrategyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the analysis data.
        /// </summary>
        public Dictionary<string, object> Analysis { get; set; } = new();

        /// <summary>
        /// Gets the step metrics.
        /// </summary>
        public IReadOnlyCollection<StepMetrics> StepMetrics => _stepMetrics;
        private readonly List<StepMetrics> _stepMetrics = new();

        /// <summary>
        /// Gets the resource usage metrics.
        /// </summary>
        public IReadOnlyDictionary<string, ResourceUsage> ResourceUsage => _resourceUsage;
        private readonly Dictionary<string, ResourceUsage> _resourceUsage = new();

        /// <summary>
        /// Gets the performance metrics.
        /// </summary>
        public IReadOnlyDictionary<string, PerformanceMetrics> PerformanceMetrics { get; } = new Dictionary<string, PerformanceMetrics>();

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the total duration in milliseconds.
        /// </summary>
        public double TotalDurationMs => (EndTime - StartTime).TotalMilliseconds;

        /// <summary>
        /// Gets or sets the success rate (0-1).
        /// </summary>
        public double SuccessRate => StepsExecuted > 0 ? (double)SuccessfulSteps / StepsExecuted : 0;

        /// <summary>
        /// Gets or sets the failure rate (0-1).
        /// </summary>
        public double FailureRate => StepsExecuted > 0 ? (double)FailedSteps / StepsExecuted : 0;

        /// <summary>
        /// Gets or sets the metrics dictionary.
        /// </summary>
        public Dictionary<string, double> Metrics { get; set; } = new();

        /// <summary>
        /// Adds a metric value.
        /// </summary>
        public void AddValue(string key, double value)
        {
            Values[key] = value;
        }

        /// <summary>
        /// Adds a metric label.
        /// </summary>
        public void AddLabel(string key, string value)
        {
            Labels[key] = value;
        }

        /// <summary>
        /// Adds step metrics.
        /// </summary>
        public void AddStepMetrics(StepMetrics metrics) => _stepMetrics.Add(metrics);

        /// <summary>
        /// Adds resource usage metrics.
        /// </summary>
        public void AddResourceUsage(string key, ResourceUsage usage) => _resourceUsage[key] = usage;
    }
} 
