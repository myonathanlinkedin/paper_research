using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Models.LLM
{
    /// <summary>
    /// Represents a suggestion from the LLM.
    /// </summary>
    public class LLMSuggestion
    {
        /// <summary>
        /// Gets or sets the unique identifier of the suggestion.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the action to be taken.
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the priority of the suggestion.
        /// </summary>
        public RemediationPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the impact of the suggestion.
        /// </summary>
        public string Impact { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the risk level of the suggestion.
        /// </summary>
        public RemediationRiskLevel Risk { get; set; }

        /// <summary>
        /// Gets or sets the validation requirements.
        /// </summary>
        public string Validation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confidence score.
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the suggestion.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the step-by-step instructions for implementing the suggestion.
        /// </summary>
        public List<string> Steps { get; set; } = new();
        
        /// <summary>
        /// Gets or sets additional metadata about the suggestion.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        public string ErrorId { get; set; } = string.Empty;
        public string CorrelationId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
} 
