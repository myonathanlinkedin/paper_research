using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Analysis
{
    /// <summary>
    /// Represents a root cause analysis for an error.
    /// </summary>
    public class RootCauseAnalysis
    {
        /// <summary>
        /// Gets or sets the unique identifier of the analysis.
        /// </summary>
        public string AnalysisId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        public ErrorContext Context { get; }

        /// <summary>
        /// Gets or sets the primary root cause.
        /// </summary>
        public string PrimaryRootCause { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the possible root causes with confidence levels.
        /// </summary>
        public Dictionary<string, double> PossibleRootCauses { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// Gets or sets the root cause confidence level (0-1).
        /// </summary>
        public double ConfidenceLevel { get; }

        /// <summary>
        /// Gets or sets the severity of the root cause.
        /// </summary>
        public SeverityLevel Severity { get; } = SeverityLevel.Medium;

        /// <summary>
        /// Gets or sets the causal chain of events.
        /// </summary>
        public IReadOnlyCollection<CausalChain> CausalChain { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the contributing factors.
        /// </summary>
        public IReadOnlyCollection<ContributingFactors> ContributingFactors { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the timestamp of the analysis.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the additional context.
        /// </summary>
        public Dictionary<string, object> AdditionalContext { get; set; } = new Dictionary<string, object>();
    }
} 






