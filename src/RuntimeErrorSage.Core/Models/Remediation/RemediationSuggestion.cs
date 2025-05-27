using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents a remediation suggestion for an error.
    /// </summary>
    public class RemediationSuggestion
    {
        /// <summary>
        /// Gets or sets the unique identifier of the suggestion.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the suggestion identifier.
        /// </summary>
        public string SuggestionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the title of the suggestion.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the action to be taken.
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the priority of the suggestion.
        /// </summary>
        public RemediationPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the impact of the remediation.
        /// </summary>
        public RemediationImpact Impact { get; set; } = new();

        /// <summary>
        /// Gets or sets the risk level of the remediation.
        /// </summary>
        public RemediationRiskLevel Risk { get; set; }

        /// <summary>
        /// Gets or sets the validation requirements.
        /// </summary>
        public RemediationValidation Validation { get; set; } = new();

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error ID.
        /// </summary>
        public string ErrorId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp of the suggestion.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the confidence score.
        /// </summary>
        public double ConfidenceScore { get; set; }

        /// <summary>
        /// Gets or sets the list of actions.
        /// </summary>
        public List<RemediationAction> Actions { get; set; } = new();
    }
} 