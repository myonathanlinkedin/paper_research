using System.Collections.Generic;
using System.Threading.Tasks;
using CodeSage.Core.Models.Error;
using CodeSage.Core.Models.Metrics;

namespace CodeSage.Core.Interfaces
{
    /// <summary>
    /// Defines the interface for collecting remediation metrics.
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
    }
} 