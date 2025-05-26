using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Interface for collecting and managing remediation metrics.
    /// </summary>
    public interface IRemediationMetricsCollector
    {
        /// <summary>
        /// Gets whether the metrics collector is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the metrics collector name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the metrics collector version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Collects metrics for a remediation operation.
        /// </summary>
        /// <param name="context">The error context</param>
        /// <returns>A dictionary of metrics</returns>
        Task<Dictionary<string, object>> CollectMetricsAsync(ErrorContext context);

        /// <summary>
        /// Records a metric value for a specific remediation.
        /// </summary>
        /// <param name="remediationId">The remediation ID</param>
        /// <param name="metricName">The metric name</param>
        /// <param name="value">The metric value</param>
        Task RecordMetricAsync(string remediationId, string metricName, object value);

        /// <summary>
        /// Gets the metrics history for a specific remediation.
        /// </summary>
        /// <param name="remediationId">The remediation ID</param>
        /// <returns>A dictionary of metric histories</returns>
        Task<Dictionary<string, List<MetricValue>>> GetMetricsHistoryAsync(string remediationId);

        /// <summary>
        /// Records metrics for a remediation operation.
        /// </summary>
        /// <param name="metrics">The remediation metrics.</param>
        Task RecordRemediationMetricsAsync(RemediationMetrics metrics);

        /// <summary>
        /// Records metrics for a remediation step.
        /// </summary>
        /// <param name="metrics">The step metrics.</param>
        Task RecordStepMetricsAsync(StepMetrics metrics);

        /// <summary>
        /// Gets the current metrics for a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation ID.</param>
        /// <returns>The remediation metrics.</returns>
        Task<RemediationMetrics> GetRemediationMetricsAsync(string remediationId);

        /// <summary>
        /// Gets the aggregated metrics for a time period.
        /// </summary>
        /// <param name="range">The time range.</param>
        /// <returns>The aggregated metrics.</returns>
        Task<RemediationMetrics> GetAggregatedMetricsAsync(TimeRange range);

        /// <summary>
        /// Validates metrics against theoretical thresholds.
        /// </summary>
        /// <param name="metrics">The remediation metrics.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateMetricsAsync(RemediationMetrics metrics);

        /// <summary>
        /// Gets metrics for a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation identifier.</param>
        /// <returns>The remediation metrics.</returns>
        Task<RemediationMetrics> GetMetricsAsync(string remediationId);
    }

    /// <summary>
    /// Represents remediation metrics.
    /// </summary>
    public class RemediationMetrics
    {
        /// <summary>
        /// Gets or sets the remediation identifier.
        /// </summary>
        public required string RemediationId { get; set; }

        /// <summary>
        /// Gets or sets the remediation status.
        /// </summary>
        public RemediationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        public Dictionary<string, double> Values { get; set; } = new();
    }

    /// <summary>
    /// Represents step metrics.
    /// </summary>
    public class StepMetrics
    {
        /// <summary>
        /// Gets or sets the step identifier.
        /// </summary>
        public string StepId { get; set; }

        /// <summary>
        /// Gets or sets the step name.
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// Gets or sets the step status.
        /// </summary>
        public RemediationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the step duration in milliseconds.
        /// </summary>
        public double DurationMs { get; set; }

        /// <summary>
        /// Gets or sets the step metrics.
        /// </summary>
        public Dictionary<string, double> Values { get; set; } = new();
    }
} 
