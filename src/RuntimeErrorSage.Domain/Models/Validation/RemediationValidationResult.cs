using System;
using System.Collections.Generic;
using RuntimeErrorSage.Model.Models.Error;

namespace RuntimeErrorSage.Model.Models.Validation
{
    /// <summary>
    /// Represents the result of validating a remediation.
    /// </summary>
    public class RemediationValidationResult
    {
        /// <summary>
        /// Gets or sets whether the validation was successful.
        /// </summary>
        public bool IsValid { get; set; }
        
        /// <summary>
        /// Gets or sets the error context associated with the validation.
        /// </summary>
        public ErrorContext ErrorContext { get; set; }
        
        /// <summary>
        /// Gets or sets the timestamp of the validation.
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// Gets or sets the validation messages, typically error messages if IsValid is false.
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();
        
        /// <summary>
        /// Gets or sets additional validation results.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new List<ValidationResult>();
        
        /// <summary>
        /// Gets or sets any errors that occurred during validation.
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
} 
