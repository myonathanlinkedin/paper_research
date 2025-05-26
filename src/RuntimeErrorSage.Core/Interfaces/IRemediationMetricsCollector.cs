using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Validation;
using RemediationStatus = RuntimeErrorSage.Core.Remediation.Models.Common.RemediationStatus;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Defines the interface for collecting and managing remediation metrics.
    /// </summary>
    public interface IRemediationMetricsCollector
    {
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
        Task RecordRemediationMetricsAsync(RemediationMetrics metrics);

        /// <summary>
        /// Records metrics for a remediation step.
        /// </summary>
        Task RecordStepMetricsAsync(StepMetrics metrics);

        /// <summary>
        /// Gets the current metrics for a remediation operation.
        /// </summary>
        Task<RemediationMetrics> GetRemediationMetricsAsync(string remediationId);

        /// <summary>
        /// Gets the aggregated metrics for a time period.
        /// </summary>
        Task<RemediationMetrics> GetAggregatedMetricsAsync(TimeRange range);

        /// <summary>
        /// Validates metrics against theoretical thresholds.
        /// </summary>
        Task<ValidationResult> ValidateMetricsAsync(RemediationMetrics metrics);
    }

    /// <summary>
    /// Represents a time range for metrics aggregation.
    /// </summary>
    public class TimeRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    /// <summary>
    /// Represents metrics for a remediation step.
    /// </summary>
    public class StepMetrics
    {
        public string StepId { get; set; }
        public string ActionId { get; set; }
        public double Duration { get; set; }
        public RemediationStatus Status { get; set; }
        public string ErrorType { get; set; }
    }
} 
