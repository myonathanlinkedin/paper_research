using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Enums;

// Use alias to avoid ambiguity
using GraphComponentDependency = RuntimeErrorSage.Domain.Models.Graph.ComponentDependency;
using ErrorComponentDependency = RuntimeErrorSage.Domain.Models.Error.ComponentDependency;

namespace RuntimeErrorSage.Core.Graph
{
    /// <summary>
    /// Extension methods for graph edges to resolve property access issues.
    /// </summary>
    public static class GraphEdgeExtensions
    {
        /// <summary>
        /// Gets the source ID from a KeyValuePair of string and object where the object is a GraphEdge or ComponentDependency.
        /// </summary>
        /// <param name="pair">The KeyValuePair to extract from.</param>
        /// <returns>The source ID.</returns>
        public static string GetSourceId(this KeyValuePair<string, object> pair)
        {
            if (pair.Value is GraphEdge edge)
            {
                return edge.Source?.Id ?? string.Empty;
            }
            else if (pair.Value is GraphComponentDependency dependency)
            {
                return dependency.SourceComponentId;
            }
            else if (pair.Value is ErrorComponentDependency errorDependency)
            {
                return errorDependency.Source;
            }
            else if (pair.Value is DependencyEdge depEdge)
            {
                return depEdge.SourceId;
            }
            
            return pair.Key.Split('_')[0];
        }

        /// <summary>
        /// Gets the target ID from a KeyValuePair of string and object where the object is a GraphEdge or ComponentDependency.
        /// </summary>
        /// <param name="pair">The KeyValuePair to extract from.</param>
        /// <returns>The target ID.</returns>
        public static string GetTargetId(this KeyValuePair<string, object> pair)
        {
            if (pair.Value is GraphEdge edge)
            {
                return edge.Target?.Id ?? string.Empty;
            }
            else if (pair.Value is GraphComponentDependency dependency)
            {
                return dependency.TargetComponentId;
            }
            else if (pair.Value is ErrorComponentDependency errorDependency)
            {
                return errorDependency.Target;
            }
            else if (pair.Value is DependencyEdge depEdge)
            {
                return depEdge.TargetId;
            }
            
            return pair.Key.Split('_')[1];
        }

        /// <summary>
        /// Gets the source ID from an object that might be a GraphEdge or ComponentDependency.
        /// </summary>
        /// <param name="obj">The object to extract from.</param>
        /// <returns>The source ID.</returns>
        public static string GetSourceId(this object obj)
        {
            if (obj is GraphEdge edge)
            {
                return edge.Source?.Id ?? string.Empty;
            }
            else if (obj is GraphComponentDependency dependency)
            {
                return dependency.SourceComponentId;
            }
            else if (obj is ErrorComponentDependency errorDependency)
            {
                return errorDependency.Source;
            }
            else if (obj is KeyValuePair<string, object> pair)
            {
                return GetSourceId(pair);
            }
            else if (obj is DependencyEdge depEdge)
            {
                return depEdge.SourceId;
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Gets the target ID from an object that might be a GraphEdge or ComponentDependency.
        /// </summary>
        /// <param name="obj">The object to extract from.</param>
        /// <returns>The target ID.</returns>
        public static string GetTargetId(this object obj)
        {
            if (obj is GraphEdge edge)
            {
                return edge.Target?.Id ?? string.Empty;
            }
            else if (obj is GraphComponentDependency dependency)
            {
                return dependency.TargetComponentId;
            }
            else if (obj is ErrorComponentDependency errorDependency)
            {
                return errorDependency.Target;
            }
            else if (obj is KeyValuePair<string, object> pair)
            {
                return GetTargetId(pair);
            }
            else if (obj is DependencyEdge depEdge)
            {
                return depEdge.TargetId;
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Converts a GraphNode to a DependencyNode.
        /// </summary>
        /// <param name="graphNode">The GraphNode to convert.</param>
        /// <returns>A DependencyNode with equivalent properties.</returns>
        public static DependencyNode ToDependencyNode(this GraphNode graphNode)
        {
            if (graphNode == null)
            {
                return null;
            }
            
            var dependencyNode = new DependencyNode
            {
                Id = graphNode.Id,
                Name = graphNode.Name,
                NodeType = graphNode.NodeType,
                IsCritical = graphNode.IsCritical,
                ErrorProbability = graphNode.ErrorProbability
            };
            
            dependencyNode.Metadata = graphNode.Metadata;
            
            return dependencyNode;
        }
        
        /// <summary>
        /// Extension method to get a path between nodes in a dependency graph.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        /// <param name="sourceId">The source node ID.</param>
        /// <param name="targetId">The target node ID.</param>
        /// <returns>A path between the nodes.</returns>
        public static GraphPath GetPath(this DependencyGraph graph, string sourceId, string targetId)
        {
            return graph?.FindPath(sourceId, targetId) ?? new GraphPath();
        }
        
        /// <summary>
        /// Converts a GraphNodeType enum to a string representation
        /// </summary>
        /// <param name="nodeType">The GraphNodeType to convert</param>
        /// <returns>String representation of the enum value</returns>
        public static string ToString(this GraphNodeType nodeType)
        {
            return nodeType.ToString();
        }

        /// <summary>
        /// Checks if the graph has an edge between the specified nodes.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        /// <param name="sourceId">The source node ID.</param>
        /// <param name="targetId">The target node ID.</param>
        /// <returns>True if the edge exists, false otherwise.</returns>
        public static bool HasEdge(this DependencyGraph graph, string sourceId, string targetId)
        {
            if (graph == null || string.IsNullOrEmpty(sourceId) || string.IsNullOrEmpty(targetId))
            {
                return false;
            }

            return graph.Edges.Any(e => e.Value().SourceId == sourceId && e.Value().TargetId == targetId);
        }

        /// <summary>
        /// Gets the Value from a KeyValuePair of string and GraphEdge.
        /// This method is needed because Value is a method in some contexts.
        /// </summary>
        /// <param name="pair">The KeyValuePair containing a GraphEdge.</param>
        /// <returns>The GraphEdge value.</returns>
        public static GraphEdge Value(this KeyValuePair<string, GraphEdge> pair)
        {
            return pair.Value;
        }
        
        /// <summary>
        /// Gets the Value from a KeyValuePair of string and DependencyEdge.
        /// This method is needed because Value is a method in some contexts.
        /// </summary>
        /// <param name="pair">The KeyValuePair containing a DependencyEdge.</param>
        /// <returns>The DependencyEdge value.</returns>
        public static DependencyEdge Value(this KeyValuePair<string, DependencyEdge> pair)
        {
            return pair.Value;
        }
    }
} 