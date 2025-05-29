using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Validation
{
    /// <summary>
    /// Represents the validation result for a remediation step.
    /// </summary>
    public class StepValidationResult
    {
        /// <summary>
        /// Gets or sets whether the step is valid.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Gets or sets the reason for validation failure.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Gets or sets the confidence score for the validation (0-1).
        /// </summary>
        public double ConfidenceScore { get; }

        /// <summary>
        /// Gets or sets additional validation details.
        /// </summary>
        public Dictionary<string, object> ValidationDetails { get; set; } = new();
    }
} 







