using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Metrics
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

        /// <summary>
        /// Gets or sets the tags associated with the metric.
        /// </summary>
        public HashSet<string> Tags { get; set; } = new HashSet<string>();

        /// <summary>
        /// Gets or sets the unit of measurement for the metric.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the description of the metric.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the metric.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the source of the metric.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the namespace of the metric.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the metadata associated with the metric.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }
} 
