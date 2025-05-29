using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Graph;

namespace RuntimeErrorSage.Application.Models.Analysis
{
    /// <summary>
    /// Represents the result of a graph analysis operation.
    /// </summary>
    public class GraphAnalysisResult
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
        /// Gets or sets the status of the analysis.
        /// </summary>
        public AnalysisStatus Status { get; } = AnalysisStatus.NotStarted;

        /// <summary>
        /// Gets or sets the root node of the dependency graph.
        /// </summary>
        public DependencyNode RootNode { get; }

        /// <summary>
        /// Gets or sets the related errors.
        /// </summary>
        public IReadOnlyCollection<RelatedErrors> RelatedErrors { get; } = new Collection<RelatedError>();

        /// <summary>
        /// Gets or sets the timestamp of the analysis.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the start time of the analysis.
        /// </summary>
        public DateTime StartTime { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end time of the analysis.
        /// </summary>
        public DateTime EndTime { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the component ID.
        /// </summary>
        public string ComponentId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the analysis is valid.
        /// </summary>
        public bool IsValid { get; } = true;

        /// <summary>
        /// Gets or sets the error message if the analysis is invalid.
        /// </summary>
        public string ErrorMessage { get; } = string.Empty;
    }
} 





