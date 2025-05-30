using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Validation
{
    /// <summary>
    /// Represents validation of metrics.
    /// </summary>
    public class MetricsValidation
    {
        /// <summary>
        /// Gets or sets the unique identifier for this validation.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the metric being validated.
        /// </summary>
        public string MetricName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current value of the metric.
        /// </summary>
        public double CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets the threshold value for the metric.
        /// </summary>
        public double Threshold { get; set; }

        /// <summary>
        /// Gets or sets the comparison operator used for validation.
        /// </summary>
        public string ComparisonOperator { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the metric validation passed.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the validation was performed.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation severity.
        /// </summary>
        public string Severity { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional metadata about the validation.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
} 

