using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Validation
{
    /// <summary>
    /// Represents metrics for validation operations.
    /// </summary>
    public class MetricsValidation
    {
        /// <summary>
        /// Gets or sets the duration of the validation in milliseconds.
        /// </summary>
        public long DurationMs { get; set; }

        /// <summary>
        /// Gets or sets the number of rules validated.
        /// </summary>
        public int RulesValidated { get; set; }

        /// <summary>
        /// Gets or sets the number of errors found.
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// Gets or sets the number of warnings found.
        /// </summary>
        public int WarningCount { get; set; }

        /// <summary>
        /// Gets or sets the memory usage during validation.
        /// </summary>
        public long MemoryUsage { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the metrics were collected.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets additional metrics.
        /// </summary>
        public Dictionary<string, object> AdditionalMetrics { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets whether the validation is valid.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the validation metadata.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
} 

