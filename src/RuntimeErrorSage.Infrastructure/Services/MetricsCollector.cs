using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Models.Metrics;
using RuntimeErrorSage.Application.Models.Common;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Graph;

namespace RuntimeErrorSage.Application.Services
{
    /// <summary>
    /// Collects and records metrics about system operations.
    /// </summary>
    public class MetricsCollector : RuntimeErrorSage.Application.Metrics.Interfaces.IMetricsCollector
    {
        private readonly ILogger<MetricsCollector> _logger;
        private readonly Dictionary<string, List<MetricEntry>> _metrics = new Dictionary<string, List<MetricEntry>>();

        /// <summary>
        /// Gets whether the collector is enabled.
        /// </summary>
        public bool IsEnabled { get; } = true;

        /// <summary>
        /// Gets the collector name.
        /// </summary>
        public string Name { get; } = "StandardMetricsCollector";

        /// <summary>
        /// Gets the collector version.
        /// </summary>
        public string Version { get; } = "1.0.0";

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricsCollector"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MetricsCollector(ILogger<MetricsCollector> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Records a metric value.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <param name="value">The metric value.</param>
        public async Task RecordMetricAsync(string name, double value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Metric name cannot be null or empty", nameof(name));
            }

            try
            {
                var metric = new MetricEntry
                {
                    Name = name,
                    Value = value,
                    Timestamp = DateTime.UtcNow
                };

                if (!_metrics.ContainsKey(name))
                {
                    _metrics[name] = new List<MetricEntry>();
                }

                _metrics[name].Add(metric);
                _logger.LogDebug("Recorded metric {MetricName} with value {Value}", name, value);
                
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording metric {MetricName}", name);
                throw;
            }
        }

        /// <summary>
        /// Gets metric values.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <param name="range">The time range.</param>
        /// <returns>The metric values.</returns>
        public async Task<IEnumerable<double>> GetMetricValuesAsync(string name, TimeRange range)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Metric name cannot be null or empty", nameof(name));
            }

            if (range == null)
            {
                throw new ArgumentNullException(nameof(range));
            }

            try
            {
                if (!_metrics.ContainsKey(name))
                {
                    return Enumerable.Empty<double>();
                }

                var values = _metrics[name]
                    .Where(m => m.Timestamp >= range.Start && m.Timestamp <= range.End)
                    .Select(m => m.Value)
                    .ToList();

                await Task.CompletedTask;
                return values;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting metric values for {MetricName}", name);
                throw;
            }
        }

        /// <summary>
        /// Gets aggregated metric values.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <param name="range">The time range.</param>
        /// <param name="aggregation">The aggregation type.</param>
        /// <returns>The aggregated metric value.</returns>
        public async Task<double> GetAggregatedMetricAsync(string name, TimeRange range, AggregationType aggregation)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Metric name cannot be null or empty", nameof(name));
            }

            if (range == null)
            {
                throw new ArgumentNullException(nameof(range));
            }

            try
            {
                var values = await GetMetricValuesAsync(name, range);
                
                if (!values.Any())
                {
                    return 0.0;
                }

                switch (aggregation)
                {
                    case AggregationType.Sum:
                        return values.Sum();
                    case AggregationType.Average:
                        return values.Average();
                    case AggregationType.Min:
                        return values.Min();
                    case AggregationType.Max:
                        return values.Max();
                    case AggregationType.Count:
                        return values.Count();
                    default:
                        throw new ArgumentException($"Unsupported aggregation type: {aggregation}", nameof(aggregation));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating aggregated metric for {MetricName}", name);
                throw;
            }
        }

        /// <summary>
        /// Calculates the health score of a component node.
        /// </summary>
        /// <param name="node">The dependency node.</param>
        /// <returns>The health score (0.0 to 1.0).</returns>
        public async Task<double> CalculateComponentHealthAsync(DependencyNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            try
            {
                // Example implementation - in a real system, this would calculate health based on metrics
                // For now, we'll just return a random value
                var random = new Random();
                var healthScore = random.NextDouble();
                
                await Task.CompletedTask;
                return healthScore;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating health score for node {NodeId}", node.Id);
                throw;
            }
        }

        /// <summary>
        /// Calculates the reliability score of a component node.
        /// </summary>
        /// <param name="node">The dependency node.</param>
        /// <returns>The reliability score (0.0 to 1.0).</returns>
        public async Task<double> CalculateReliabilityAsync(DependencyNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            try
            {
                // Example implementation - in a real system, this would calculate reliability based on metrics
                // For now, we'll just return a random value
                var random = new Random();
                var reliabilityScore = 0.7 + (random.NextDouble() * 0.3); // 0.7 to 1.0 range
                
                await Task.CompletedTask;
                return reliabilityScore;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating reliability score for node {NodeId}", node.Id);
                throw;
            }
        }

        /// <summary>
        /// Represents a metric entry.
        /// </summary>
        private class MetricEntry
        {
            /// <summary>
            /// Gets or sets the metric name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the metric value.
            /// </summary>
            public double Value { get; set; }

            /// <summary>
            /// Gets or sets the timestamp when the metric was recorded.
            /// </summary>
            public DateTime Timestamp { get; set; }
        }
    }
} 
