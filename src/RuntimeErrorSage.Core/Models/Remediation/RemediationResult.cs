using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the result of a remediation operation.
    /// </summary>
    public class RemediationResult
    {
        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        public ErrorContext Context { get; set; }

        /// <summary>
        /// Gets or sets the remediation status.
        /// </summary>
        public RemediationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the remediation message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the completed remediation steps.
        /// </summary>
        public List<string> CompletedSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the failed remediation steps.
        /// </summary>
        public List<string> FailedSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the remediation metrics.
        /// </summary>
        public Dictionary<string, double> Metrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation result.
        /// </summary>
        public ValidationResult Validation { get; set; }

        /// <summary>
        /// Gets or sets the action taken.
        /// </summary>
        public string ActionTaken { get; set; }

        /// <summary>
        /// Gets or sets when the remediation was performed.
        /// </summary>
        public DateTime RemediationTimestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Represents the impact of a remediation operation.
    /// </summary>
    public class RemediationImpact
    {
        // Other properties...

        /// <summary>
        /// Gets additional impact details.
        /// </summary>
        public Dictionary<string, object> Details { get; } = new();

        // Other properties...
    }
} 