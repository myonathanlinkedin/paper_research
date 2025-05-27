using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Validation
{
    /// <summary>
    /// Represents the result of validating a remediation operation.
    /// </summary>
    public class RemediationValidationResult
    {
        /// <summary>
        /// Gets or sets whether the validation was successful.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation results.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new();
    }
} 