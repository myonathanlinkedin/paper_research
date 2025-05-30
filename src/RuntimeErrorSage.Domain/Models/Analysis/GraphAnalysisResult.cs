using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Graph;

namespace RuntimeErrorSage.Domain.Models.Analysis
{
    /// <summary>
    /// Represents the result of a graph analysis operation.
    /// </summary>
    public class GraphAnalysisResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for this analysis result.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the timestamp when the analysis was performed.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the analyzed dependency graph.
        /// </summary>
        public DependencyGraph Graph { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of identified critical paths.
        /// </summary>
        public List<List<DependencyNode>> CriticalPaths { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of identified cycles in the graph.
        /// </summary>
        public List<List<DependencyNode>> Cycles { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of isolated nodes in the graph.
        /// </summary>
        public List<DependencyNode> IsolatedNodes { get; set; } = new();

        /// <summary>
        /// Gets or sets the metrics for this analysis.
        /// </summary>
        public GraphAnalysisMetrics Metrics { get; set; } = new();

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

    /// <summary>
    /// Represents metrics for a graph analysis operation.
    /// </summary>
    public class GraphAnalysisMetrics
    {
        /// <summary>
        /// Gets or sets the total number of nodes in the graph.
        /// </summary>
        public int TotalNodes { get; set; }

        /// <summary>
        /// Gets or sets the total number of edges in the graph.
        /// </summary>
        public int TotalEdges { get; set; }

        /// <summary>
        /// Gets or sets the number of critical paths found.
        /// </summary>
        public int CriticalPathCount { get; set; }

        /// <summary>
        /// Gets or sets the number of cycles found.
        /// </summary>
        public int CycleCount { get; set; }

        /// <summary>
        /// Gets or sets the number of isolated nodes found.
        /// </summary>
        public int IsolatedNodeCount { get; set; }

        /// <summary>
        /// Gets or sets the execution time of the analysis in milliseconds.
        /// </summary>
        public double ExecutionTimeMs { get; set; }
    }
} 
