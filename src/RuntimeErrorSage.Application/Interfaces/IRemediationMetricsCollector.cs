using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Validation;
using RuntimeErrorSage.Application.Models.Common;
using RuntimeErrorSage.Application.Models.Metrics;

namespace RuntimeErrorSage.Application.Interfaces
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
    Task<Dictionary<string, Collection<Models.Remediation.MetricValue>>> GetMetricsHistoryAsync(string remediationId);

        /// <summary>
        /// Records the execution of a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation ID</param>
        /// <param name="result">The remediation result</param>
        Task RecordExecutionAsync(string remediationId, RemediationResult result);

        /// <summary>
        /// Records the rollback of a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation ID</param>
        /// <param name="result">The remediation result</param>
        Task RecordRollbackAsync(string remediationId, RemediationResult result);

        /// <summary>
        /// Gets the remediation result for a specific ID.
        /// </summary>
        /// <param name="remediationId">The remediation ID</param>
        /// <returns>The remediation result</returns>
        Task<RemediationResult> GetRemediationResultAsync(string remediationId);

        /// <summary>
        /// Records the execution of a remediation action.
        /// </summary>
        /// <param name="actionId">The action ID</param>
        /// <param name="result">The action result</param>
        Task RecordActionExecutionAsync(string actionId, RemediationActionResult result);

        /// <summary>
        /// Records the rollback of a remediation action.
        /// </summary>
        /// <param name="actionId">The action ID</param>
        /// <param name="result">The action result</param>
        Task RecordActionRollbackAsync(string actionId, RemediationActionResult result);

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
        Task<RemediationMetrics> GetMetricsAsync(string remediationId);

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
    }
} 







