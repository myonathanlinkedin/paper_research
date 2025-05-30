using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Analysis;
using RuntimeErrorSage.Domain.Models.Remediation;

namespace RuntimeErrorSage.Core.MCP
{
    /// <summary>
    /// Represents the result of a context analysis by the Model Context Protocol.
    /// </summary>
    public class ContextAnalysisResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for this analysis result.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the correlation identifier for tracing.
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the analysis was performed.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the status of the analysis.
        /// </summary>
        public AnalysisStatus Status { get; set; } = AnalysisStatus.Pending;

        /// <summary>
        /// Gets or sets the error analysis result.
        /// </summary>
        public ErrorAnalysisResult ErrorAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the remediation suggestions.
        /// </summary>
        public List<RemediationSuggestion> Suggestions { get; set; } = new();

        /// <summary>
        /// Gets or sets additional details about the analysis.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();

        /// <summary>
        /// Gets or sets the confidence level of the analysis.
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the error context that was analyzed.
        /// </summary>
        public string ContextId { get; set; }

        /// <summary>
        /// Gets or sets the execution time of the analysis.
        /// </summary>
        public TimeSpan ExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the model information used for the analysis.
        /// </summary>
        public string ModelInfo { get; set; }
    }
} 