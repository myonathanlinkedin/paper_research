using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Remediation;

namespace RuntimeErrorSage.Application.Models.LLM
{
    /// <summary>
    /// Represents a suggestion from the LLM.
    /// </summary>
    public class LLMSuggestion
    {
        /// <summary>
        /// Gets or sets the unique identifier of the suggestion.
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the action to be taken.
        /// </summary>
        public string Action { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the priority of the suggestion.
        /// </summary>
        public RemediationPriority Priority { get; }

        /// <summary>
        /// Gets or sets the impact of the suggestion.
        /// </summary>
        public string Impact { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the risk level of the suggestion.
        /// </summary>
        public RemediationRiskLevel Risk { get; }

        /// <summary>
        /// Gets or sets the validation requirements.
        /// </summary>
        public string Validation { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the confidence score.
        /// </summary>
        public double Confidence { get; }

        /// <summary>
        /// Gets or sets the timestamp of the suggestion.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the step-by-step instructions for implementing the suggestion.
        /// </summary>
        public IReadOnlyCollection<Steps> Steps { get; } = new();
        
        /// <summary>
        /// Gets or sets additional metadata about the suggestion.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        public string ErrorId { get; } = string.Empty;
        public string CorrelationId { get; } = string.Empty;
        public string Description { get; } = string.Empty;
    }
} 






