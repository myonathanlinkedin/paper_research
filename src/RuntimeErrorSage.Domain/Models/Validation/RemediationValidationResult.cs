using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Error;

namespace RuntimeErrorSage.Application.Models.Validation
{
    /// <summary>
    /// Represents the result of validating a remediation.
    /// </summary>
    public class RemediationValidationResult
    {
        /// <summary>
        /// Gets or sets whether the validation was successful.
        /// </summary>
        public bool IsValid { get; }
        
        /// <summary>
        /// Gets or sets the error context associated with the validation.
        /// </summary>
        public ErrorContext ErrorContext { get; }
        
        /// <summary>
        /// Gets or sets the timestamp of the validation.
        /// </summary>
        public DateTime Timestamp { get; }
        
        /// <summary>
        /// Gets or sets the validation messages, typically error messages if IsValid is false.
        /// </summary>
        public IReadOnlyCollection<Messages> Messages { get; } = new Collection<string>();
        
        /// <summary>
        /// Gets or sets additional validation results.
        /// </summary>
        public IReadOnlyCollection<ValidationResults> ValidationResults { get; } = new Collection<ValidationResult>();
        
        /// <summary>
        /// Gets or sets any errors that occurred during validation.
        /// </summary>
        public IReadOnlyCollection<Errors> Errors { get; } = new Collection<string>();
    }
} 






