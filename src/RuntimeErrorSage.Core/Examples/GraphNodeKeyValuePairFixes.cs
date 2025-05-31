using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Core.Graph;

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
            //    string id = node.Id;  // Error: node is a KeyValuePair<string, GraphNode>
            //    string type = node.Type.ToString();  // Error: KeyValuePair has no Type property
            // }

            // CORRECT: Access through Value property
            foreach (var nodeEntry in graph.Nodes)
            {
                var node = nodeEntry.Value;
                string id = node.Id;
                
                // Get type safely
                string type = node.Type;
                
                // Or use pattern matching
                if (node.NodeType == GraphNodeType.Service)
                {
                    Console.WriteLine($"Found service node: {id}");
                }
                
                // Access other properties through Value
                Console.WriteLine($"Node {id} has {node.Metadata?.Count ?? 0} properties");
            }
        }

        /// <summary>
        /// Shows correct ways to access edge properties through KeyValuePair.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        public void ShowEdgeDictionaryAccess(DependencyGraph graph)
        {
            // CORRECT: Use Value method for access
            foreach (var edgeEntry in graph.Edges)
            {
                var edge = edgeEntry.Value();
                
                // Get edge data using type-specific properties
                if (edge is DependencyEdge dependencyEdge)
                {
                    string sourceId = dependencyEdge.SourceId;
                    string targetId = dependencyEdge.TargetId;
                    double weight = dependencyEdge.Weight;
                    Console.WriteLine($"Dependency edge from {sourceId} to {targetId} with weight {weight}");
                }
                else
                {
                    // Use extension methods for other edge types
                    string sourceId = edge.GetSourceId();
                    string targetId = edge.GetTargetId();
                    Console.WriteLine($"Edge from {sourceId} to {targetId}");
                }
            }
            
            // Another correct way: use LINQ projection
            var dependencies = graph.Edges
                .Select(e => e.Value())
                .Where(e => e is DependencyEdge)
                .Cast<DependencyEdge>()
                .ToList();
            
            Console.WriteLine($"Found {dependencies.Count} dependency edges");
        }

        /// <summary>
        /// Shows how to work with impact analysis results.
        /// </summary>
        /// <param name="result">The impact analysis result.</param>
        public void ShowImpactAnalysisAccess(ImpactAnalysisResult result)
        {
            // Correct way to access affected nodes and their impact values
            if (result.AffectedNodesMap != null)
            {
                foreach (var nodeEntry in result.AffectedNodesMap)
                {
                    string nodeId = nodeEntry.Key;
                    double impactProbability = nodeEntry.Value;
                    
                    Console.WriteLine($"Node {nodeId} has impact probability {impactProbability}");
                }
            }
            
            // Iterate over other collections - safely check for null first
            if (result.AffectedNodes != null)
            {
                foreach (var nodeEntry in result.AffectedNodes)
                {
                    var node = nodeEntry.Value;
                    // Use null conditional to safely access severity if it exists
                    Console.WriteLine($"Node {node.Id} is affected");
                }
            }

            // CORRECT: Using LINQ without implicit KeyValuePair access - check for null first
            if (result.AffectedNodesMap != null)
            {
                var criticalNodeIds = result.AffectedNodesMap
                    .Where(n => n.Value > 0.8)
                    .Select(n => n.Key)
                    .ToList();

                Console.WriteLine($"Found {criticalNodeIds.Count} critical node dependencies");
            }
            
            // Accessing specific nodes by key - safely check for null first
            if (result.AffectedNodesMap != null && result.AffectedNodesMap.TryGetValue("key1", out var impactValue))
            {
                Console.WriteLine($"Node 'key1' has impact value {impactValue}");
            }
        }
    }
} 

