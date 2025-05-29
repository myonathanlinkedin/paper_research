using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.LLM
{
    /// <summary>
    /// Represents insights derived from graph analysis.
    /// </summary>
    public class GraphInsights
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the timestamp of the analysis.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the confidence level of the insights (0-1).
        /// </summary>
        public double Confidence { get; }

        /// <summary>
        /// Gets or sets the severity of the insights.
        /// </summary>
        public SeverityLevel Severity { get; }

        /// <summary>
        /// Gets or sets the main insight message.
        /// </summary>
        public string Message { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the detailed description of the insights.
        /// </summary>
        public string Description { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of affected components.
        /// </summary>
        public IReadOnlyCollection<AffectedComponents> AffectedComponents { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the list of suggested actions.
        /// </summary>
        public IReadOnlyCollection<SuggestedActions> SuggestedActions { get; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the metrics associated with the insights.
        /// </summary>
        public Dictionary<string, double> Metrics { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// Gets or sets additional metadata about the insights.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }
} 





