using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Graph
{
    /// <summary>
    /// Represents an edge in a dependency graph.
    /// </summary>
    public class DependencyEdge
    {
        /// <summary>
        /// Gets or sets the unique identifier of the edge.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the source node ID.
        /// </summary>
        public string SourceId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the target node ID.
        /// </summary>
        public string TargetId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the relationship type.
        /// </summary>
        public RelationshipType RelationshipType { get; set; }

        /// <summary>
        /// Gets or sets the edge weight (0-1).
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the edge label.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the edge is directed.
        /// </summary>
        public bool IsDirected { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the edge is critical.
        /// </summary>
        public bool IsCritical { get; set; }

        /// <summary>
        /// Gets or sets the edge metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }
} 
