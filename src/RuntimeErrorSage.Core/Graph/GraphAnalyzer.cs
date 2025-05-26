using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Context;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Graph;

/// <summary>
/// Implements the graph-based context analysis as specified in the research paper.
/// This implementation follows the mathematical framework described in Section 3.
/// </summary>
public class GraphAnalyzer : IGraphAnalyzer
{
    private readonly ILogger<GraphAnalyzer> _logger;
    private readonly IErrorClassifier _errorClassifier;
    private readonly IMetricsCollector _metricsCollector;

    public GraphAnalyzer(
        ILogger<GraphAnalyzer> logger,
        IErrorClassifier errorClassifier,
        IMetricsCollector metricsCollector)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _errorClassifier = errorClassifier ?? throw new ArgumentNullException(nameof(errorClassifier));
        _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
    }

    /// <summary>
    /// Builds a dependency graph from the runtime context.
    /// Implements the graph construction algorithm described in Section 3.1.
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
    /// Implements the impact analysis algorithm described in Section 3.2.
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

        // Calculate impact metrics as described in Section 3.2
        result.ImpactMetrics["severity"] = await CalculateImpactSeverityAsync(graph, result.AffectedNodes);
        result.ImpactMetrics["spread"] = (double)result.AffectedNodes.Count / graph.Nodes.Count;
        result.ImpactMetrics["recency"] = await CalculateRecencyAsync(graph, errorNodeId);
        result.ImpactMetrics["importance"] = await CalculateImportanceAsync(graph, errorNodeId);
        result.ImpactMetrics["connectivity"] = await CalculateConnectivityAsync(graph, errorNodeId);
        result.ImpactMetrics["error_proximity"] = await CalculateErrorProximityAsync(graph, errorNodeId);

        return result;
    }

    private async Task TraverseImpactAsync(DependencyGraph graph, string nodeId, HashSet<string> visited, ImpactAnalysisResult result)
    {
        if (visited.Contains(nodeId))
            return;

        visited.Add(nodeId);
        result.AffectedNodes.Add(nodeId);

        // Find all outgoing edges
        var outgoingEdges = graph.Edges.Where(e => e.SourceId == nodeId);
        foreach (var edge in outgoingEdges)
        {
            await TraverseImpactAsync(graph, edge.TargetId, visited, result);
        }
    }

    private async Task<double> CalculateEdgeWeightAsync(Dependency dependency)
    {
        // Calculate edge weight based on dependency strength and type
        var baseWeight = dependency.Strength;
        var typeMultiplier = GetDependencyTypeMultiplier(dependency.Type);
        return baseWeight * typeMultiplier;
    }

    private double GetDependencyTypeMultiplier(string dependencyType)
    {
        return dependencyType switch
        {
            "direct" => 1.0,
            "indirect" => 0.7,
            "optional" => 0.3,
            _ => 0.5
        };
    }

    private async Task<double> CalculateImpactSeverityAsync(DependencyGraph graph, List<string> affectedNodes)
    {
        // Calculate severity based on affected nodes' error probabilities
        var totalSeverity = 0.0;
        foreach (var nodeId in affectedNodes)
        {
            var node = graph.Nodes.FirstOrDefault(n => n.Id == nodeId);
            if (node != null)
            {
                totalSeverity += node.ErrorProbability;
            }
        }
        return totalSeverity / affectedNodes.Count;
    }

    private async Task<double> CalculateRecencyAsync(DependencyGraph graph, string nodeId)
    {
        // Calculate recency based on node's last update timestamp
        var node = graph.Nodes.FirstOrDefault(n => n.Id == nodeId);
        if (node == null || !node.Properties.ContainsKey("lastUpdate"))
            return 0.0;

        var lastUpdate = (DateTime)node.Properties["lastUpdate"];
        var timeSinceUpdate = DateTime.UtcNow - lastUpdate;
        return Math.Exp(-timeSinceUpdate.TotalMinutes / 60.0); // Exponential decay
    }

    private async Task<double> CalculateImportanceAsync(DependencyGraph graph, string nodeId)
    {
        // Calculate importance based on node centrality
        var node = graph.Nodes.FirstOrDefault(n => n.Id == nodeId);
        if (node == null)
            return 0.0;

        var inDegree = graph.Edges.Count(e => e.TargetId == nodeId);
        var outDegree = graph.Edges.Count(e => e.SourceId == nodeId);
        var totalNodes = graph.Nodes.Count;

        return (inDegree + outDegree) / (double)(2 * (totalNodes - 1));
    }

    private async Task<double> CalculateConnectivityAsync(DependencyGraph graph, string nodeId)
    {
        // Calculate connectivity based on node's edge weights
        var edges = graph.Edges.Where(e => e.SourceId == nodeId || e.TargetId == nodeId);
        return edges.Sum(e => e.Weight) / edges.Count();
    }

    private async Task<double> CalculateErrorProximityAsync(DependencyGraph graph, string nodeId)
    {
        // Calculate error proximity based on shortest path to error nodes
        var errorNodes = graph.Nodes.Where(n => n.ErrorProbability > 0.5).ToList();
        if (!errorNodes.Any())
            return 0.0;

        var minDistance = double.MaxValue;
        foreach (var errorNode in errorNodes)
        {
            var distance = await CalculateShortestPathAsync(graph, nodeId, errorNode.Id);
            minDistance = Math.Min(minDistance, distance);
        }

        return Math.Exp(-minDistance); // Exponential decay
    }

    private async Task<double> CalculateShortestPathAsync(DependencyGraph graph, string sourceId, string targetId)
    {
        // Implement Dijkstra's algorithm for shortest path
        var distances = new Dictionary<string, double>();
        var visited = new HashSet<string>();
        var queue = new PriorityQueue<string, double>();

        foreach (var node in graph.Nodes)
        {
            distances[node.Id] = double.MaxValue;
        }
        distances[sourceId] = 0;
        queue.Enqueue(sourceId, 0);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current == targetId)
                break;

            if (visited.Contains(current))
                continue;

            visited.Add(current);

            var edges = graph.Edges.Where(e => e.SourceId == current);
            foreach (var edge in edges)
            {
                var newDistance = distances[current] + (1.0 / edge.Weight);
                if (newDistance < distances[edge.TargetId])
                {
                    distances[edge.TargetId] = newDistance;
                    queue.Enqueue(edge.TargetId, newDistance);
                }
            }
        }

        return distances[targetId];
    }
} 