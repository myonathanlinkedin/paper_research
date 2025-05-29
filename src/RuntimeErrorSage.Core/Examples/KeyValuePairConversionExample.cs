using RuntimeErrorSage.Model.Models.Graph;

namespace RuntimeErrorSage.Model.Examples;

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
        
        // CORRECT: Extract the value from KeyValuePair
        if (graph.Nodes.Any())
        {
            var kvp = graph.Nodes.First();
            DependencyNode node = kvp.Value;
            
            // Now use the node...
            Console.WriteLine($"Node ID: {node.Id}");
        }
        
        // ALTERNATIVE: Use LINQ Select to transform collection
        var dependencyNodes = graph.Nodes.Select(kvp => kvp.Value).ToList();
        
        // Now you have a list of DependencyNode objects
        foreach (var node in dependencyNodes)
        {
            Console.WriteLine($"Node ID: {node.Id}");
        }
    }
} 
