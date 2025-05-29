using System;
using System.Collections.Generic;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Model.Models.Graph;

namespace RuntimeErrorSage.Model.Models.Analysis
{
    /// <summary>
    /// Represents the result of a graph analysis operation.
    /// </summary>
    public class GraphAnalysisResult
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
        /// Gets or sets the status of the analysis.
        /// </summary>
        public AnalysisStatus Status { get; set; } = AnalysisStatus.NotStarted;

        /// <summary>
        /// Gets or sets the root node of the dependency graph.
        /// </summary>
        public DependencyNode RootNode { get; set; }

        /// <summary>
        /// Gets or sets the related errors.
        /// </summary>
        public List<RelatedError> RelatedErrors { get; set; } = new List<RelatedError>();

        /// <summary>
        /// Gets or sets the timestamp of the analysis.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the start time of the analysis.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end time of the analysis.
        /// </summary>
        public DateTime EndTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component ID.
        /// </summary>
        public string ComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the analysis is valid.
        /// </summary>
        public bool IsValid { get; set; } = true;

        /// <summary>
        /// Gets or sets the error message if the analysis is invalid.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;
    }
} 
