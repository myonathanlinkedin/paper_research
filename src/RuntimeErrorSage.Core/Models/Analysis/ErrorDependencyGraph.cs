using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Models.Analysis
{
    /// <summary>
    /// Represents a dependency graph of related errors.
    /// </summary>
    public class ErrorDependencyGraph
    {
        /// <summary>
        /// Gets or sets the unique identifier of the graph.
        /// </summary>
        public string GraphId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the nodes in the graph.
        /// </summary>
        public List<DependencyNode> Nodes { get; set; } = new List<DependencyNode>();

        /// <summary>
        /// Gets or sets the edges in the graph.
        /// </summary>
        public List<DependencyEdge> Edges { get; set; } = new List<DependencyEdge>();

        /// <summary>
        /// Gets or sets the root error node.
        /// </summary>
        public DependencyNode RootNode { get; set; }

        /// <summary>
        /// Gets or sets the related errors.
        /// </summary>
        public List<RelatedError> RelatedErrors { get; set; } = new List<RelatedError>();

        /// <summary>
        /// Gets or sets the error relationships.
        /// </summary>
        public List<ErrorRelationship> Relationships { get; set; } = new List<ErrorRelationship>();

        /// <summary>
        /// Gets or sets the timestamp of the graph creation.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Adds a node to the graph.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(DependencyNode node)
        {
            ArgumentNullException.ThrowIfNull(node);
            Nodes.Add(node);
        }

        /// <summary>
        /// Adds an edge to the graph.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        public void AddEdge(DependencyEdge edge)
        {
            ArgumentNullException.ThrowIfNull(edge);
            Edges.Add(edge);
        }

        /// <summary>
        /// Gets the dependencies of a node.
        /// </summary>
        /// <param name="nodeId">The node ID.</param>
        /// <returns>The dependencies.</returns>
        public List<DependencyNode> GetDependencies(string nodeId)
        {
            ArgumentNullException.ThrowIfNull(nodeId);
            var dependencies = new List<DependencyNode>();
            foreach (var edge in Edges)
            {
                if (edge.SourceId == nodeId)
                {
                    var targetNode = Nodes.Find(n => n.Id == edge.TargetId);
                    if (targetNode != null)
                    {
                        dependencies.Add(targetNode);
                    }
                }
            }
            return dependencies;
        }

        /// <summary>
        /// Gets the dependents of a node.
        /// </summary>
        /// <param name="nodeId">The node ID.</param>
        /// <returns>The dependents.</returns>
        public List<DependencyNode> GetDependents(string nodeId)
        {
            var dependents = new List<DependencyNode>();
            foreach (var edge in Edges)
            {
                if (edge.TargetId == nodeId)
                {
                    var sourceNode = Nodes.Find(n => n.Id == edge.SourceId);
                    if (sourceNode != null)
                    {
                        dependents.Add(sourceNode);
                    }
                }
            }
            return dependents;
        }
    }
} 
