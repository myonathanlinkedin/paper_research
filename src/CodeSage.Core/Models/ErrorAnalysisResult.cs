using System;
using System.Collections.Generic;

namespace CodeSage.Core.Models
{
    /// <summary>
    /// Represents the result of an error analysis.
    /// </summary>
    public class ErrorAnalysisResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the analysis.
        /// </summary>
        public string AnalysisId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the correlation ID of the error context.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the analysis was performed.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the identified root cause of the error.
        /// </summary>
        public string RootCause { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confidence level of the analysis (0-1).
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the suggested remediation actions.
        /// </summary>
        public List<RemediationAction> SuggestedActions { get; set; } = new();

        /// <summary>
        /// Gets or sets additional analysis metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the analysis is complete.
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Gets or sets any error that occurred during analysis.
        /// </summary>
        public string? AnalysisError { get; set; }
    }
} 