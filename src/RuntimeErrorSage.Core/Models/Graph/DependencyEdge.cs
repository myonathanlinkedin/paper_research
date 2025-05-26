using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Represents an edge in the dependency graph.
    /// </summary>
    public class DependencyEdge
    {
        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// Gets or sets the target node identifier.
        /// </summary>
        public string TargetId { get; set; }

        /// <summary>
        /// Gets or sets the edge type.
        /// </summary>
        public string EdgeType { get; set; }

        /// <summary>
        /// Gets or sets the edge weight.
        /// </summary>
        public double Weight { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the edge metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
} 