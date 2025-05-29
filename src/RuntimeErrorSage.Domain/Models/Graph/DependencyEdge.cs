using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Graph
{
    /// <summary>
    /// Represents an edge in a dependency graph.
    /// </summary>
    public class DependencyEdge
    {
        /// <summary>
        /// Gets or sets the unique identifier of the edge.
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the source node ID.
        /// </summary>
        public string SourceId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the target node ID.
        /// </summary>
        public string TargetId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the relationship type.
        /// </summary>
        public RelationshipType RelationshipType { get; }

        /// <summary>
        /// Gets or sets the edge weight (0-1).
        /// </summary>
        public double Weight { get; }

        /// <summary>
        /// Gets or sets the edge label.
        /// </summary>
        public string Label { get; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the edge is directed.
        /// </summary>
        public bool IsDirected { get; } = true;

        /// <summary>
        /// Gets or sets whether the edge is critical.
        /// </summary>
        public bool IsCritical { get; }

        /// <summary>
        /// Gets or sets the edge metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }
} 






