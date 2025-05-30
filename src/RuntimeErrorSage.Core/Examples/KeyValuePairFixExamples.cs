using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Core.Graph;

namespace RuntimeErrorSage.Application.Examples
{
    /// <summary>
    /// Examples showing how to properly access properties through KeyValuePair
    /// </summary>
    public class KeyValuePairFixExamples
    {
        /// <summary>
        /// Shows correct vs incorrect ways to access GraphNode properties from a Dictionary KeyValuePair
        /// </summary>
        /// <param name="graph">The dependency graph</param>
        public void ShowNodeDictionaryAccess(DependencyGraph graph)
        {
            // INCORRECT: This causes "'KeyValuePair<string, GraphNode>' does not contain a definition for 'Id'"
            // foreach (var node in graph.Nodes)
            // {
            //     Console.WriteLine($"Node ID: {node.Id}");
            // }
            
            // CORRECT: Access the Value property of the KeyValuePair
            foreach (var node in graph.Nodes)
            {
                Console.WriteLine($"Node ID: {node.Value.Id}");
                Console.WriteLine($"Node Type: {node.Value.Type}");
            }
            
            // ALTERNATIVE: Use the dictionary's Values collection directly
            foreach (var nodeValue in graph.Nodes.Values())
            {
                Console.WriteLine($"Node ID: {nodeValue.Id}");
                Console.WriteLine($"Node Type: {nodeValue.Type}");
            }
            
            // ALTERNATIVE: Access using KeyValuePair explicitly
            foreach (var nodePair in graph.Nodes)
            {
                var key = nodePair.Key;
                var nodeValue = nodePair.Value;
                Console.WriteLine($"Key: {key}, Node ID: {nodeValue.Id}");
            }
        }
        
        /// <summary>
        /// Shows correct vs incorrect ways to access GraphEdge properties
        /// </summary>
        /// <param name="graph">The dependency graph</param>
        public void ShowEdgeDictionaryAccess(DependencyGraph graph)
        {
            // INCORRECT: Trying to access properties directly on GraphEdge
            // foreach (var edge in graph.Edges)
            // {
            //     Console.WriteLine($"Edge source: {edge.SourceId}, target: {edge.TargetId}");
            // }
            
            // CORRECT: Access the properties correctly
            foreach (var edge in graph.Edges)
            {
                // GraphEdge objects have Source and Target properties
                var source = edge.Source?.Id ?? "";
                var target = edge.Target?.Id ?? "";
                Console.WriteLine($"Edge source: {source}, target: {target}");
            }
        }
        
        /// <summary>
        /// Shows how to properly access DependencyNode properties in impact analysis
        /// </summary>
        /// <param name="result">Impact analysis result</param>
        public void ShowImpactAnalysisAccess(ImpactAnalysisResult result)
        {
            // Access DirectDependencies correctly - these are already DependencyNode objects, not KeyValuePairs
            foreach (var dependency in result.DirectDependencies)
            {
                // Correct: Access properties directly since this isn't a KeyValuePair
                Console.WriteLine($"Dependency ID: {dependency.Id}");
                
                // Access metadata if available
                if (dependency.Metadata != null && 
                    dependency.Metadata.TryGetValue("ErrorProbability", out var probability))
                {
                    // Handle string value from metadata
                    if (double.TryParse(probability.ToString(), out double errorProb))
                    {
                        Console.WriteLine($"Error probability: {errorProb}");
                    }
                }
            }
            
            // The commented lines below now reference properties that exist in RuntimeErrorSage.Core.Models.Graph.ImpactAnalysisResult
            // var metrics = result.ImpactMetrics; // Now valid since we're using RuntimeErrorSage.Core.Models.Graph.ImpactAnalysisResult
            // var nodes = result.AffectedNodes; // Now valid since we're using RuntimeErrorSage.Core.Models.Graph.ImpactAnalysisResult
            
            // CORRECT: Use the properties that actually exist on ImpactAnalysisResult
            Console.WriteLine($"Component ID: {result.ComponentId}");
            Console.WriteLine($"Severity: {result.Severity}");
            // Verify what properties actually exist on ImpactAnalysisResult
            // Console.WriteLine($"Scope: {result.Scope}");
            Console.WriteLine($"Impact Score: {result.TotalImpactScore}");
        }
    }
} 
