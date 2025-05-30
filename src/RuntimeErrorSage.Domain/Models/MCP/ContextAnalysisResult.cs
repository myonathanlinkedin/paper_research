using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Models.Context;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.MCP
{
    /// <summary>
    /// Represents the result of a context analysis operation.
    /// </summary>
    public class ContextAnalysisResult
    {
        /// <summary>
        /// Gets or sets the context identifier.
        /// </summary>
        public string ContextId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp of the analysis.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the context metadata.
        /// </summary>
        public ContextMetadata Metadata { get; set; } = new ContextMetadata();

        /// <summary>
        /// Gets or sets the dependency graph.
        /// </summary>
        public DependencyGraph DependencyGraph { get; set; }

        /// <summary>
        /// Gets or sets the status of the analysis.
        /// </summary>
        public AnalysisStatus Status { get; set; } = AnalysisStatus.NotStarted;

        /// <summary>
        /// Gets or sets additional details about the analysis.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new Dictionary<string, object>();
    }
} 
