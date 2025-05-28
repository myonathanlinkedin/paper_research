using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Services.Models
{
    /// <summary>
    /// Represents a metric value with its unit and metadata.
    /// </summary>
    public class MetricValue
    {
        /// <summary>
        /// Gets or sets the metric value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the metric unit.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the metric metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 
