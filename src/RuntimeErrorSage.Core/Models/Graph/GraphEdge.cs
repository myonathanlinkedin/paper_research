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
        /// Gets or sets the unique identifier for this edge.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the ID of the source node.
        /// </summary>
        public string SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the target node.
        /// </summary>
        public string TargetNodeId { get; set; }

        /// <summary>
        /// Gets or sets the label of the edge.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the weight of the edge.
        /// </summary>
        public double Weight { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets whether the edge is directed.
        /// </summary>
        public bool IsDirected { get; set; } = true;

        /// <summary>
        /// Gets or sets metadata associated with the edge.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the creation time of the edge.
        /// </summary>
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphEdge"/> class.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="target">The target node.</param>
        /// <param name="type">The edge type.</param>
        public GraphEdge(GraphNode source, GraphNode target, GraphEdgeType type)
        {
            SourceNodeId = source?.Id;
            TargetNodeId = target?.Id;
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