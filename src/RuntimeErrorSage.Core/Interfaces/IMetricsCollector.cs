using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Interface for collecting metrics.
    /// </summary>
    public interface IMetricsCollector
    {
        /// <summary>
        /// Gets whether the collector is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the collector name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the collector version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Records a metric value.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <param name="value">The metric value.</param>
        /// <param name="labels">Optional metric labels.</param>
        Task RecordMetricAsync(string name, double value, Dictionary<string, string> labels = null);

        /// <summary>
        /// Records multiple metric values.
        /// </summary>
        /// <param name="metrics">The metrics to record.</param>
        Task RecordMetricsAsync(Dictionary<string, MetricValue> metrics);

        /// <summary>
        /// Gets metric values for a time range.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <param name="range">The time range.</param>
        /// <returns>The metric values.</returns>
        Task<List<MetricValue>> GetMetricValuesAsync(string name, TimeRange range);

        /// <summary>
        /// Gets aggregated metric values.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <param name="range">The time range.</param>
        /// <param name="aggregation">The aggregation type.</param>
        /// <returns>The aggregated metric value.</returns>
        Task<double> GetAggregatedMetricAsync(string name, TimeRange range, AggregationType aggregation);
    }

    /// <summary>
    /// Represents a metric value.
    /// </summary>
    public class MetricValue
    {
        /// <summary>
        /// Gets or sets the metric name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the metric value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the metric labels.
        /// </summary>
        public required Dictionary<string, string> Labels { get; set; }
    }

    /// <summary>
    /// Represents a time range.
    /// </summary>
    public class TimeRange
    {
        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        public DateTime End { get; set; }
    }

    /// <summary>
    /// Specifies the type of metric aggregation.
    /// </summary>
    public enum AggregationType
    {
        /// <summary>
        /// Average of values.
        /// </summary>
        Average,

        /// <summary>
        /// Sum of values.
        /// </summary>
        Sum,

        /// <summary>
        /// Minimum value.
        /// </summary>
        Min,

        /// <summary>
        /// Maximum value.
        /// </summary>
        Max,

        /// <summary>
        /// Count of values.
        /// </summary>
        Count
    }
} 