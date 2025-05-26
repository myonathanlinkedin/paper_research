using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Context;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Graph;

/// <summary>
/// Implements the graph-based context analysis as specified in the research paper.
/// </summary>
public class GraphAnalyzer : IGraphAnalyzer
{
    private readonly IErrorClassifier _errorClassifier;
    private readonly IMetricsCollector _metricsCollector;

    public GraphAnalyzer(IErrorClassifier errorClassifier, IMetricsCollector metricsCollector)
    {
        _errorClassifier = errorClassifier ?? throw new ArgumentNullException(nameof(errorClassifier));
        _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
    }

    /// <summary>
    /// Builds a dependency graph from the runtime context.
    /// </summary>
    public async Task<DependencyGraph> BuildDependencyGraphAsync(RuntimeContext context)
    {
        var graph = new DependencyGraph
        {
            Nodes = new List<DependencyNode>(),
            Edges = new List<DependencyEdge>(),
            Metadata = new Dictionary<string, object>()
        };

        // Extract nodes from runtime context
        foreach (var component in context.Components)
        {
            var node = new DependencyNode
            {
                Id = component.Id,
                Type = component.Type,
                Properties = component.Properties,
                ErrorProbability = await _errorClassifier.CalculateErrorProbabilityAsync(component)
            };
            graph.Nodes.Add(node);
        }

        // Build edges based on component dependencies
        foreach (var dependency in context.Dependencies)
        {
            var edge = new DependencyEdge
            {
                SourceId = dependency.SourceId,
                TargetId = dependency.TargetId,
                Type = dependency.Type,
                Weight = await CalculateEdgeWeightAsync(dependency)
            };
            graph.Edges.Add(edge);
        }

        // Add graph-level metrics
        graph.Metadata["complexity"] = await _metricsCollector.CalculateComplexityAsync(graph);
        graph.Metadata["reliability"] = await _metricsCollector.CalculateReliabilityAsync(graph);
        graph.Metadata["timestamp"] = DateTime.UtcNow;

        return graph;
    }

    /// <summary>
    /// Analyzes the impact of an error through the dependency graph.
    /// </summary>
    public async Task<ImpactAnalysisResult> AnalyzeImpactAsync(DependencyGraph graph, string errorNodeId)
    {
        var result = new ImpactAnalysisResult
        {
            ErrorNodeId = errorNodeId,
            AffectedNodes = new List<string>(),
            ImpactMetrics = new Dictionary<string, double>()
        };

        var visited = new HashSet<string>();
        await TraverseImpactAsync(graph, errorNodeId, visited, result);

        result.ImpactMetrics["severity"] = await CalculateImpactSeverityAsync(graph, result.AffectedNodes);
        result.ImpactMetrics["spread"] = (double)result.AffectedNodes.Count / graph.Nodes.Count;

        return result;
    }

    private async Task TraverseImpactAsync(DependencyGraph graph, string nodeId, HashSet<string> visited, ImpactAnalysisResult result)
    {
        if (visited.Contains(nodeId))
            return;

        visited.Add(nodeId);
        result.AffectedNodes.Add(nodeId);

        var outgoingEdges = graph.Edges.FindAll(e => e.SourceId == nodeId);
        foreach (var edge in outgoingEdges)
        {
            await TraverseImpactAsync(graph, edge.TargetId, visited, result);
        }
    }

    private async Task<double> CalculateEdgeWeightAsync(DependencyInfo dependency)
    {
        // Implement edge weight calculation based on dependency characteristics
        return await Task.FromResult(1.0); // Default implementation
    }

    private async Task<double> CalculateImpactSeverityAsync(DependencyGraph graph, List<string> affectedNodes)
    {
        // Implement impact severity calculation based on affected nodes and their properties
        return await Task.FromResult(1.0); // Default implementation
    }
} 