using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Remediation
{
    /// <summary>
    /// Options for the remediation validator.
    /// </summary>
    public class RemediationValidatorOptions
    {
        /// <summary>
        /// Gets or sets whether the validator is enabled.
        /// </summary>
        public bool IsEnabled { get; } = true;

        /// <summary>
        /// Gets or sets the timeout for validation operations.
        /// </summary>
        public TimeSpan ValidationTimeout { get; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Gets or sets the minimum health score for system health validation.
        /// </summary>
        public double MinimumHealthScore { get; } = 70.0;
    }
} 






