using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Metrics
{
    /// <summary>
    /// Represents a metric entry.
    /// </summary>
    public class MetricEntry
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

        /// <summary>
        /// Gets or sets the tags associated with this metric.
        /// </summary>
        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
    }
} 
