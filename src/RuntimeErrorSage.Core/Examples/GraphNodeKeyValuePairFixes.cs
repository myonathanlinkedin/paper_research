using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Graph;

namespace RuntimeErrorSage.Core.Examples
{
    /// <summary>
    /// Example code showing how to fix KeyValuePair access issues.
    /// </summary>
    public class GraphNodeKeyValuePairFixes
    {
        /// <summary>
        /// Shows incorrect vs correct ways to access GraphNode properties through KeyValuePair.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        public void ShowGraphNodeFixExample(DependencyGraph graph)
        {
            // INCORRECT: This will cause "KeyValuePair<string, GraphNode> does not contain a definition for 'Id'"
            // foreach (var node in graph.Nodes)
            // {
            //     Console.WriteLine($"Node ID: {node.Id}");
            //     Console.WriteLine($"Node Type: {node.NodeType}");
            //     Console.WriteLine($"Error Probability: {node.ErrorProbability}");
            // }

            // CORRECT: Access the Value property of the KeyValuePair
            foreach (var node in graph.Nodes)
            {
                Console.WriteLine($"Node ID: {node.Value.Id}");
                Console.WriteLine($"Node Type: {node.Value.Type}");
                
                // For properties that might not exist in GraphNode but in a derived type,
                // use a safe approach with dictionaries
                if (node.Value.Metadata != null && 
                    node.Value.Metadata.TryGetValue("ErrorProbability", out var probability))
                {
                    // Check for double using proper type check instead of pattern matching
                    if (probability is double)
                    {
                        double errorProb = (double)probability;
                        Console.WriteLine($"Error Probability: {errorProb}");
                    }
                }
            }

            // ALTERNATIVE: Use LINQ to work with the Values directly
            foreach (var node in graph.Nodes.Values)
            {
                Console.WriteLine($"Node ID: {node.Id}");
                Console.WriteLine($"Node Type: {node.Type}");
            }
        }

        /// <summary>
        /// Shows incorrect vs correct ways to access GraphEdge properties through KeyValuePair.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        public void ShowGraphEdgeFixExample(DependencyGraph graph)
        {
            // INCORRECT: This will cause "KeyValuePair<string, GraphEdge> does not contain a definition for 'SourceId'"
            // foreach (var edge in graph.Edges)
            // {
            //     Console.WriteLine($"Edge Source ID: {edge.SourceId}");
            //     Console.WriteLine($"Edge Target ID: {edge.TargetId}");
            // }

            // CORRECT: Access the Value property of the KeyValuePair
            foreach (var edge in graph.Edges)
            {
                Console.WriteLine($"Edge Source ID: {edge.Value.SourceId}");
                Console.WriteLine($"Edge Target ID: {edge.Value.TargetId}");
            }

            // ALTERNATIVE: Use LINQ to work with the Values directly
            foreach (var edge in graph.Edges.Values)
            {
                Console.WriteLine($"Edge Source ID: {edge.SourceId}");
                Console.WriteLine($"Edge Target ID: {edge.TargetId}");
            }
        }

        /// <summary>
        /// Shows how to fix KeyValuePair issues when getting neighbors.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        /// <param name="nodeId">The node ID to get neighbors for.</param>
        public void ShowGetNeighborsFixExample(DependencyGraph graph, string nodeId)
        {
            // Get the node's neighbors
            var neighbors = graph.GetNeighbors(nodeId);
            
            // Process neighbors (already a List<GraphNode>, so no KeyValuePair issues)
            foreach (var neighbor in neighbors)
            {
                Console.WriteLine($"Neighbor ID: {neighbor.Id}");
                Console.WriteLine($"Neighbor Type: {neighbor.Type}");
            }
        }

        /// <summary>
        /// Shows how to fix issues when working with impact analysis results.
        /// </summary>
        /// <param name="result">The impact analysis result.</param>
        public void ShowImpactAnalysisFixExample(ImpactAnalysisResult result)
        {
            // Process direct dependencies (already a List<DependencyNode>, so no KeyValuePair issues)
            foreach (var dependency in result.DirectDependencies)
            {
                Console.WriteLine($"Dependency ID: {dependency.Id}");
                Console.WriteLine($"Dependency Type: {dependency.NodeType}");
                Console.WriteLine($"Error Probability: {dependency.ErrorProbability}");
            }
        }
    }
} 
