using System;
using System.Linq;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Graph;

namespace RuntimeErrorSage.Core.Examples;

/// <summary>
/// Demonstrates proper KeyValuePair to DependencyNode conversion patterns
/// </summary>
public static class KeyValuePairConversionExample
{
    /// <summary>
    /// Shows correct conversion from KeyValuePair to DependencyNode
    /// </summary>
    public static void KeyValuePairToDependencyNodeConversion()
    {
        var graph = new DependencyGraph();
        
        // INCORRECT: Direct conversion from KeyValuePair to DependencyNode
        /*
        DependencyNode node = graph.Nodes.First(); // Error: Cannot convert KeyValuePair to DependencyNode
        */
        
        // CORRECT: Extract the value from KeyValuePair and handle the type
        if (graph.Nodes.Any())
        {
            var kvp = graph.Nodes.First();
            GraphNode graphNode = kvp.Value;
            
            // Create a new DependencyNode from the GraphNode's properties
            var dependencyNode = new DependencyNode
            {
                Id = graphNode.Id,
                NodeType = graphNode.Type,
                Metadata = graphNode.Metadata
            };
            
            // Now use the node...
            Console.WriteLine($"Node ID: {dependencyNode.Id}");
        }
        
        // ALTERNATIVE: Use LINQ Select to transform collection
        var dependencyNodes = graph.Nodes
            .Select(kvp => new DependencyNode 
            {
                Id = kvp.Value.Id,
                NodeType = kvp.Value.Type,
                Metadata = kvp.Value.Metadata
            })
            .ToList();
        
        // Now you have a list of DependencyNode objects
        foreach (var node in dependencyNodes)
        {
            Console.WriteLine($"Node ID: {node.Id}");
        }
    }
} 
