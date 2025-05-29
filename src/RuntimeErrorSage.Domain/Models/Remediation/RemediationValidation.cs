using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents the validation requirements for a remediation action.
    /// </summary>
    public class RemediationValidation
    {
        /// <summary>
        /// Gets or sets the validation rules.
        /// </summary>
        public List<string> Rules { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation timeout.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets or sets whether the validation is required.
        /// </summary>
        public bool IsRequired { get; set; }
    }
} 
