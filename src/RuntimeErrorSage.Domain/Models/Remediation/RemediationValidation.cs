using System.Collections.ObjectModel;
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
        public IReadOnlyCollection<Rules> Rules { get; } = new();

        /// <summary>
        /// Gets or sets the validation timeout.
        /// </summary>
        public TimeSpan Timeout { get; }

        /// <summary>
        /// Gets or sets whether the validation is required.
        /// </summary>
        public bool IsRequired { get; }
    }
} 






