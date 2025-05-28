using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Analysis
{
    /// <summary>
    /// Represents a root cause analysis for an error.
    /// </summary>
    public class RootCauseAnalysis
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
        /// Gets or sets the primary root cause.
        /// </summary>
        public string PrimaryRootCause { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the possible root causes with confidence levels.
        /// </summary>
        public Dictionary<string, double> PossibleRootCauses { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// Gets or sets the root cause confidence level (0-1).
        /// </summary>
        public double ConfidenceLevel { get; set; }

        /// <summary>
        /// Gets or sets the severity of the root cause.
        /// </summary>
        public SeverityLevel Severity { get; set; } = SeverityLevel.Medium;

        /// <summary>
        /// Gets or sets the causal chain of events.
        /// </summary>
        public List<string> CausalChain { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the contributing factors.
        /// </summary>
        public List<string> ContributingFactors { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the timestamp of the analysis.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

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
