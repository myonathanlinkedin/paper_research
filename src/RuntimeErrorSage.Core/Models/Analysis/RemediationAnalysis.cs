using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Models.Analysis
{
    /// <summary>
    /// Represents the analysis of remediation options for an error.
    /// </summary>
    public class RemediationAnalysis
    {
        /// <summary>
        /// Gets or sets the unique identifier of the analysis.
        /// </summary>
        public string AnalysisId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        public ErrorContext Context { get; set; }

        /// <summary>
        /// Gets or sets the list of remediation suggestions.
        /// </summary>
        public List<RemediationSuggestion> Suggestions { get; set; } = new List<RemediationSuggestion>();

        /// <summary>
        /// Gets or sets the related errors.
        /// </summary>
        public List<RelatedError> RelatedErrors { get; set; } = new List<RelatedError>();

        /// <summary>
        /// Gets or sets the error dependency graph.
        /// </summary>
        public ErrorDependencyGraph DependencyGraph { get; set; }

        /// <summary>
        /// Gets or sets the root cause analysis.
        /// </summary>
        public RootCauseAnalysis RootCause { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the analysis.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the confidence level of the analysis (0-1).
        /// </summary>
        public double ConfidenceLevel { get; set; }

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the additional context.
        /// </summary>
        public Dictionary<string, object> AdditionalContext { get; set; } = new Dictionary<string, object>();
    }
} 