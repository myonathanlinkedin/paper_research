using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Represents an edge in a dependency graph.
    /// </summary>
    public class GraphEdge
    {
        /// <summary>
        /// Gets or sets the unique identifier of the edge.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the source node of the edge.
        /// </summary>
        public GraphNode Source { get; set; }

        /// <summary>
        /// Gets or sets the target node of the edge.
        /// </summary>
        public GraphNode Target { get; set; }

        /// <summary>
        /// Gets or sets the type of the edge.
        /// </summary>
        public GraphEdgeType Type { get; set; }

        /// <summary>
        /// Gets or sets the weight of the edge.
        /// </summary>
        public double Weight { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the label of the edge.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the edge is directed.
        /// </summary>
        public bool IsDirected { get; set; } = true;

        /// <summary>
        /// Gets or sets the timestamp when the edge was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the edge was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the metadata of the edge.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphEdge"/> class.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="target">The target node.</param>
        /// <param name="type">The edge type.</param>
        public GraphEdge(GraphNode source, GraphNode target, GraphEdgeType type)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Type = type;
        }

        /// <summary>
        /// Removes this edge from its source and target nodes.
        /// </summary>
        public void Remove()
        {
            Source.RemoveOutgoingEdge(this);
            Target.RemoveIncomingEdge(this);
        }

        /// <summary>
        /// Gets the source ID of the edge.
        /// </summary>
        public string SourceId => Source?.Id;

        /// <summary>
        /// Gets the target ID of the edge.
        /// </summary>
        public string TargetId => Target?.Id;
    }
} 