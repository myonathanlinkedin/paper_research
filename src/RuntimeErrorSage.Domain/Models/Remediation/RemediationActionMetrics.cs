
using RuntimeErrorSage.Domain.Models.Remediation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Tracks and manages metrics for remediation actions.
    /// </summary>
    public class RemediationActionMetrics
    {
        private readonly Dictionary<string, double> _metrics = new();
        private readonly Dictionary<string, List<double>> _historicalMetrics = new();
        private readonly Dictionary<string, DateTime> _lastUpdated = new();

        /// <summary>
        /// Gets the current metrics.
        /// </summary>
        public IReadOnlyDictionary<string, double> Metrics => _metrics;

        /// <summary>
        /// Gets the historical metrics.
        /// </summary>
        public IReadOnlyDictionary<string, List<double>> HistoricalMetrics => _historicalMetrics;

        /// <summary>
        /// Gets the last update timestamps.
        /// </summary>
        public IReadOnlyDictionary<string, DateTime> LastUpdated => _lastUpdated;

        /// <summary>
        /// Records a metric value.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <param name="value">The metric value.</param>
        public void RecordMetric(string name, double value)
        {
            ArgumentNullException.ThrowIfNull(name);
            
            _metrics[name] = value;
            _lastUpdated[name] = DateTime.UtcNow;

            if (!_historicalMetrics.ContainsKey(name))
            {
                _historicalMetrics[name] = new List<double>();
            }
            _historicalMetrics[name].Add(value);
        }

        /// <summary>
        /// Gets the average value for a metric.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <returns>The average value.</returns>
        public double GetAverageMetric(string name)
        {
            if (!_historicalMetrics.ContainsKey(name) || _historicalMetrics[name].Count == 0)
            {
                return 0;
            }

            return _historicalMetrics[name].Average();
        }

        /// <summary>
        /// Gets the maximum value for a metric.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <returns>The maximum value.</returns>
        public double GetMaxMetric(string name)
        {
            if (!_historicalMetrics.ContainsKey(name) || _historicalMetrics[name].Count == 0)
            {
                return 0;
            }

            return _historicalMetrics[name].Max();
        }

        /// <summary>
        /// Gets the minimum value for a metric.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <returns>The minimum value.</returns>
        public double GetMinMetric(string name)
        {
            if (!_historicalMetrics.ContainsKey(name) || _historicalMetrics[name].Count == 0)
            {
                return 0;
            }

            return _historicalMetrics[name].Min();
        }

        /// <summary>
        /// Records execution time for a remediation action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="executionTimeMs">The execution time in milliseconds.</param>
        public void RecordExecutionTime(string actionId, double executionTimeMs)
        {
            RecordMetric($"ExecutionTime_{actionId}", executionTimeMs);
        }

        /// <summary>
        /// Records success rate for a remediation action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="success">Whether the action was successful.</param>
        public void RecordSuccess(string actionId, bool success)
        {
            var successRate = success ? 1.0 : 0.0;
            RecordMetric($"SuccessRate_{actionId}", successRate);
        }

        /// <summary>
        /// Records resource usage for a remediation action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="resourceType">The type of resource.</param>
        /// <param name="usage">The resource usage value.</param>
        public void RecordResourceUsage(string actionId, string resourceType, double usage)
        {
            RecordMetric($"ResourceUsage_{actionId}_{resourceType}", usage);
        }

        /// <summary>
        /// Records the impact of a remediation action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="impact">The impact value.</param>
        public void RecordImpact(string actionId, double impact)
        {
            RecordMetric($"Impact_{actionId}", impact);
        }

        /// <summary>
        /// Records the risk level of a remediation action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="riskLevel">The risk level.</param>
        public void RecordRiskLevel(string actionId, RemediationRiskLevel riskLevel)
        {
            RecordMetric($"RiskLevel_{actionId}", (double)riskLevel);
        }

        /// <summary>
        /// Records the severity of a remediation action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="severity">The severity level.</param>
        public void RecordSeverity(string actionId, RemediationActionSeverity severity)
        {
            RecordMetric($"Severity_{actionId}", (double)severity);
        }

        /// <summary>
        /// Records the status of a remediation action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="status">The status.</param>
        public void RecordStatus(string actionId, RemediationStatusEnum status)
        {
            RecordMetric($"Status_{actionId}", (double)status);
        }

        /// <summary>
        /// Gets a summary of metrics for a specific action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>A dictionary containing the metric summary.</returns>
        public Dictionary<string, object> GetActionSummary(string actionId)
        {
            var summary = new Dictionary<string, object>();

            // Get execution time metrics
            var executionTimeKey = $"ExecutionTime_{actionId}";
            if (_historicalMetrics.ContainsKey(executionTimeKey))
            {
                summary["AverageExecutionTime"] = GetAverageMetric(executionTimeKey);
                summary["MaxExecutionTime"] = GetMaxMetric(executionTimeKey);
                summary["MinExecutionTime"] = GetMinMetric(executionTimeKey);
            }

            // Get success rate
            var successRateKey = $"SuccessRate_{actionId}";
            if (_historicalMetrics.ContainsKey(successRateKey))
            {
                summary["SuccessRate"] = GetAverageMetric(successRateKey);
            }

            // Get impact
            var impactKey = $"Impact_{actionId}";
            if (_historicalMetrics.ContainsKey(impactKey))
            {
                summary["AverageImpact"] = GetAverageMetric(impactKey);
            }

            // Get risk level
            var riskLevelKey = $"RiskLevel_{actionId}";
            if (_metrics.ContainsKey(riskLevelKey))
            {
                summary["RiskLevel"] = (RemediationRiskLevel)_metrics[riskLevelKey];
            }

            // Get severity
            var severityKey = $"Severity_{actionId}";
            if (_metrics.ContainsKey(severityKey))
            {
                summary["Severity"] = (RemediationActionSeverity)_metrics[severityKey];
            }

            // Get status
            var statusKey = $"Status_{actionId}";
            if (_metrics.ContainsKey(statusKey))
            {
                summary["Status"] = (RemediationStatusEnum)_metrics[statusKey];
            }

            return summary;
        }

        /// <summary>
        /// Clears all metrics for a specific action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        public void ClearActionMetrics(string actionId)
        {
            var keysToRemove = _metrics.Keys.Where(k => k.StartsWith($"{actionId}_")).ToList();
            foreach (var key in keysToRemove)
            {
                _metrics.Remove(key);
                _historicalMetrics.Remove(key);
                _lastUpdated.Remove(key);
            }
        }

        /// <summary>
        /// Clears all metrics.
        /// </summary>
        public void ClearAllMetrics()
        {
            _metrics.Clear();
            _historicalMetrics.Clear();
            _lastUpdated.Clear();
        }
    }
}



