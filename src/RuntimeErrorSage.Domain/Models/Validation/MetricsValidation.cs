using System.Collections.Generic;

namespace RuntimeErrorSage.Model.Models.Validation
{
    /// <summary>
    /// Represents the validation results for metrics.
    /// </summary>
    public class MetricsValidation
    {
        /// <summary>
        /// Gets or sets whether the metrics are within defined thresholds.
        /// </summary>
        public bool IsWithinThresholds { get; set; }

        /// <summary>
        /// Gets or sets additional validation details.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();
    }
} 

