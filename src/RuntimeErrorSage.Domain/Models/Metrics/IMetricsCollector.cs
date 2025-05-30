using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Metrics
{
    /// <summary>
    /// Interface for collecting and managing metrics.
    /// </summary>
    public interface IMetricsCollector
    {
        /// <summary>
        /// Begins a new metrics collection scope.
        /// </summary>
        /// <returns>A disposable scope for metrics collection.</returns>
        IDisposable BeginScope();

        /// <summary>
        /// Records a metric value.
        /// </summary>
        /// <param name="name">The name of the metric.</param>
        /// <param name="value">The value to record.</param>
        void RecordMetric(string name, double value);

        /// <summary>
        /// Records a metric value with tags.
        /// </summary>
        /// <param name="name">The name of the metric.</param>
        /// <param name="value">The value to record.</param>
        /// <param name="tags">The tags associated with the metric.</param>
        void RecordMetric(string name, double value, IDictionary<string, string> tags);

        /// <summary>
        /// Increments a counter metric.
        /// </summary>
        /// <param name="name">The name of the counter.</param>
        void IncrementCounter(string name);

        /// <summary>
        /// Increments a counter metric with tags.
        /// </summary>
        /// <param name="name">The name of the counter.</param>
        /// <param name="tags">The tags associated with the counter.</param>
        void IncrementCounter(string name, IDictionary<string, string> tags);
    }
} 