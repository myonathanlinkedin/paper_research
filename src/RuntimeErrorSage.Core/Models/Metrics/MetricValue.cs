using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Metrics
{
    /// <summary>
    /// Represents a metric value with associated metadata.
    /// </summary>
    public class MetricValue
    {
        /// <summary>
        /// Gets or sets the name of the metric.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the metric.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the metric was recorded.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the labels associated with the metric.
        /// </summary>
        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();
    }
} 