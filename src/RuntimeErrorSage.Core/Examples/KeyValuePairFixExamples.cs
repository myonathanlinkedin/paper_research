using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Examples
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
            foreach (var nodeValue in graph.Nodes.Values)
            {
                Console.WriteLine($"Node ID: {nodeValue.Id}");
                Console.WriteLine($"Node Type: {nodeValue.Type}");
            }
            
            // ALTERNATIVE: Use deconstruction to get key and value separately
            foreach (var (key, nodeValue) in graph.Nodes)
            {
                Console.WriteLine($"Key: {key}, Node ID: {nodeValue.Id}");
            }
        }
        
        /// <summary>
        /// Shows correct vs incorrect ways to access GraphEdge properties from a Dictionary KeyValuePair
        /// </summary>
        /// <param name="graph">The dependency graph</param>
        public void ShowEdgeDictionaryAccess(DependencyGraph graph)
        {
            // INCORRECT: This causes "'KeyValuePair<string, GraphEdge>' does not contain a definition for 'SourceId'"
            // foreach (var edge in graph.Edges)
            // {
            //     Console.WriteLine($"Edge source: {edge.SourceId}, target: {edge.TargetId}");
            // }
            
            // CORRECT: Access the Value property of the KeyValuePair
            foreach (var edge in graph.Edges)
            {
                Console.WriteLine($"Edge source: {edge.Value.SourceId}, target: {edge.Value.TargetId}");
            }
            
            // ALTERNATIVE: Use the dictionary's Values collection directly
            foreach (var edgeValue in graph.Edges.Values)
            {
                Console.WriteLine($"Edge source: {edgeValue.SourceId}, target: {edgeValue.TargetId}");
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
                    dependency.Metadata.TryGetValue("ErrorProbability", out var probability) &&
                    probability is double errorProb)
                {
                    Console.WriteLine($"Error probability: {errorProb}");
                }
            }
            
            // INCORRECT: Trying to access non-existent properties
            // var metrics = result.ImpactMetrics; // 'ImpactAnalysisResult' does not contain definition for 'ImpactMetrics'
            // var nodes = result.AffectedNodes; // 'ImpactAnalysisResult' does not contain definition for 'AffectedNodes'
            
            // CORRECT: Use the properties that actually exist on ImpactAnalysisResult
            Console.WriteLine($"Component ID: {result.ComponentId}");
            Console.WriteLine($"Severity: {result.Severity}");
            Console.WriteLine($"Scope: {result.Scope}");
        }
    }
} 