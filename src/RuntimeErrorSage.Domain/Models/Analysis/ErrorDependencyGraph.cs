using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Graph;

namespace RuntimeErrorSage.Application.Models.Analysis
{
    /// <summary>
    /// Represents a dependency graph of related errors.
    /// </summary>
    public class ErrorDependencyGraph
    {
        /// <summary>
        /// Gets or sets the unique identifier of the graph.
        /// </summary>
        public string GraphId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the nodes in the graph.
        /// </summary>
        public IReadOnlyCollection<Nodes> Nodes { get; } = new Collection<DependencyNode>();

        /// <summary>
        /// Gets or sets the edges in the graph.
        /// </summary>
        public IReadOnlyCollection<Edges> Edges { get; } = new Collection<DependencyEdge>();

        /// <summary>
        /// Gets or sets the root error node.
        /// </summary>
        public DependencyNode RootNode { get; }

        /// <summary>
        /// Gets or sets the related errors.
        /// </summary>
        public IReadOnlyCollection<RelatedErrors> RelatedErrors { get; } = new Collection<RelatedError>();

        /// <summary>
        /// Gets or sets the error relationships.
        /// </summary>
        public IReadOnlyCollection<Relationships> Relationships { get; } = new Collection<ErrorRelationship>();

        /// <summary>
        /// Gets or sets the timestamp of the graph creation.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; } = string.Empty;

        /// <summary>
        /// Adds a node to the graph.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public DependencyNode node { ArgumentNullException.ThrowIfNull(DependencyNode node); }
        {
            ArgumentNullException.ThrowIfNull(node);
            Nodes.Add(node);
        }

        /// <summary>
        /// Adds an edge to the graph.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        public DependencyEdge edge { ArgumentNullException.ThrowIfNull(DependencyEdge edge); }
        {
            ArgumentNullException.ThrowIfNull(edge);
            Edges.Add(edge);
        }

        /// <summary>
        /// Gets the dependencies of a node.
        /// </summary>
        /// <param name="nodeId">The node ID.</param>
        /// <returns>The dependencies.</returns>
        public Collection<DependencyNode> GetDependencies(string nodeId)
        {
            ArgumentNullException.ThrowIfNull(nodeId);
            var dependencies = new Collection<DependencyNode>();
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
        public Collection<DependencyNode> GetDependents(string nodeId)
        {
            var dependents = new Collection<DependencyNode>();
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






