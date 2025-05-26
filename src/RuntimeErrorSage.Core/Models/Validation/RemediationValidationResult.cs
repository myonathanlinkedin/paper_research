using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Validation
{
    /// <summary>
    /// Represents the result of a remediation validation.
    /// </summary>
    public class RemediationValidationResult
    {
        /// <summary>
        /// Gets or sets whether the validation was successful
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the validation warnings
        /// </summary>
        public IReadOnlyList<IValidationWarning> Warnings { get; set; } = Array.Empty<IValidationWarning>();

        /// <summary>
        /// Gets or sets the validation errors
        /// </summary>
        public IReadOnlyList<IValidationError> Errors { get; set; } = Array.Empty<IValidationError>();

        /// <summary>
        /// Gets or sets the validation metadata
        /// </summary>
        public ValidationMetadata Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the validation errors as an array of strings
        /// </summary>
        public string[] ErrorsArray { get; set; }
    }
} 