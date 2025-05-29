using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Models.Metrics
{
    /// <summary>
    /// Represents a metric entry.
    /// </summary>
    public class MetricEntry
    {
        /// <summary>
        /// Gets or sets the metric name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the metric value.
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Gets or sets the timestamp when the metric was recorded.
        /// </summary>
        public DateTime Timestamp { get; }
    }
} 





