using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Graph.Interfaces;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Classifier.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Metrics;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Graph.Interfaces;

namespace RuntimeErrorSage.Application.Graph;

/// <summary>
/// Implements the graph-based context analysis as specified in the research paper.
/// This implementation follows the mathematical framework described in Section 3.
/// </summary>
public class GraphAnalyzer : IDependencyGraphAnalyzer
{
    private readonly ILogger<GraphAnalyzer> _logger;
    private readonly IGraphBuilder _graphBuilder;
    private readonly IImpactAnalyzer _impactAnalyzer;
    private readonly IErrorRelationshipAnalyzer _relationshipAnalyzer;
    private readonly IErrorClassifier _errorClassifier;
    private readonly IErrorFactory _errorFactory;

    public GraphAnalyzer(
        ILogger<GraphAnalyzer> logger,
        IGraphBuilder graphBuilder,
        IImpactAnalyzer impactAnalyzer,
        IErrorRelationshipAnalyzer relationshipAnalyzer,
        IErrorClassifier errorClassifier,
        IErrorFactory errorFactory)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(graphBuilder);
        ArgumentNullException.ThrowIfNull(impactAnalyzer);
        ArgumentNullException.ThrowIfNull(relationshipAnalyzer);
        ArgumentNullException.ThrowIfNull(errorClassifier);
        ArgumentNullException.ThrowIfNull(errorFactory);

        _logger = logger;
        _graphBuilder = graphBuilder;
        _impactAnalyzer = impactAnalyzer;
        _relationshipAnalyzer = relationshipAnalyzer;
        _errorClassifier = errorClassifier;
        _errorFactory = errorFactory;
    }

    /// <inheritdoc />
    public async Task<GraphAnalysisResult> AnalyzeContextAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Starting graph analysis for error {ErrorId}", context.ErrorId);

            var result = new GraphAnalysisResult
            {
                StartTime = DateTime.UtcNow,
                CorrelationId = context.CorrelationId,
                Timestamp = DateTime.UtcNow
            };

            // Build dependency graph
            result.DependencyGraph = await _graphBuilder.BuildGraphAsync(context);

            // Analyze impact
            var impactResults = await AnalyzeImpactAsync(result.DependencyGraph, context.ErrorId);
            result.ImpactResults = new List<ImpactAnalysisResult> { impactResults };

            // Find related errors
            result.RelatedErrors = await _relationshipAnalyzer.FindRelatedErrorsAsync(context);

            // Calculate graph metrics
            result.Metrics = await CalculateGraphMetricsAsync(result.DependencyGraph);

            result.EndTime = DateTime.UtcNow;
            result.Status = AnalysisStatus.Completed;

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during graph analysis for error {ErrorId}", context.ErrorId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<DependencyGraph> BuildDependencyGraphAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Building dependency graph for error {ErrorId}", context.ErrorId);
            return await _graphBuilder.BuildGraphAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error building dependency graph for error {ErrorId}", context.ErrorId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ImpactAnalysisResult> AnalyzeImpactAsync(DependencyGraph graph, string startNodeId)
    {
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(startNodeId);

        try
        {
            _logger.LogInformation("Analyzing impact from node {NodeId}", startNodeId);
            
            var result = new ImpactAnalysisResult
            {
                StartNodeId = startNodeId,
                Timestamp = DateTime.UtcNow,
                AffectedNodesMap = new Dictionary<string, double>()
            };

            // Find all nodes that are reachable from the start node
            var visited = new HashSet<string>();
            var queue = new Queue<string>();
            queue.Enqueue(startNodeId);
            visited.Add(startNodeId);

            while (queue.Count > 0)
            {
                var currentNodeId = queue.Dequeue();
                var currentNode = graph.Nodes.FirstOrDefault(n => n.Key == currentNodeId);
                
                if (currentNode.Equals(default(KeyValuePair<string, GraphNode>)))
                    continue;

                // Calculate impact probability for this node
                double impactProbability = 0.8; // Default value
                result.AffectedNodesMap[currentNodeId] = impactProbability;

                // Find outgoing edges
                var outgoingEdges = graph.Edges.Where(e => e.Value.SourceId == currentNodeId);
                foreach (var edge in outgoingEdges)
                {
                    if (!visited.Contains(edge.Value.TargetId))
                    {
                        visited.Add(edge.Value.TargetId);
                        queue.Enqueue(edge.Value.TargetId);
                    }
                }
            }

            result.TotalImpactScore = result.AffectedNodesMap.Values.Sum();
            result.Status = AnalysisStatus.Completed;

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing impact for node {NodeId}", startNodeId);
            throw;
        }
    }
    
    /// <inheritdoc />
    public async Task<List<GraphPath>> AnalyzeCriticalPathsAsync(DependencyGraph graph)
    {
        ArgumentNullException.ThrowIfNull(graph);

        try
        {
            _logger.LogInformation("Analyzing critical paths in dependency graph");
            
            var criticalPaths = new List<GraphPath>();
            
            // Find all terminal nodes (nodes with no outgoing edges)
            var terminalNodes = graph.Nodes
                .Where(n => !graph.Edges.Any(e => e.Value.SourceId == n.Key))
                .ToList();
            
            // For each terminal node, find the longest path to it
            foreach (var terminal in terminalNodes)
            {
                var longestPath = FindLongestPathToNode(graph, terminal.Key);
                if (longestPath.Nodes.Count > 0)
                {
                    criticalPaths.Add(longestPath);
                }
            }
            
            return criticalPaths;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing critical paths in dependency graph");
            throw;
        }
    }
    
    /// <inheritdoc />
    public async Task<List<DependencyNode>> IdentifyHighRiskNodesAsync(DependencyGraph graph, double threshold = 0.7)
    {
        ArgumentNullException.ThrowIfNull(graph);

        try
        {
            _logger.LogInformation("Identifying high-risk nodes with threshold {Threshold}", threshold);
            
            var highRiskNodes = new List<DependencyNode>();
            
            foreach (var node in graph.Nodes)
            {
                if (node.Value.ErrorProbability >= threshold)
                {
                    highRiskNodes.Add(node.Value);
                }
            }
            
            return highRiskNodes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error identifying high-risk nodes");
            throw;
        }
    }
    
    /// <inheritdoc />
    public async Task<List<GraphPath>> CalculateErrorPropagationPathsAsync(DependencyGraph graph, string errorNodeId)
    {
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(errorNodeId);

        try
        {
            _logger.LogInformation("Calculating error propagation paths from node {NodeId}", errorNodeId);
            
            var propagationPaths = new List<GraphPath>();
            var visited = new HashSet<string>();
            
            // Find all paths from the error node to terminal nodes
            FindAllPathsFromNode(graph, errorNodeId, new List<string>(), visited, propagationPaths);
            
            return propagationPaths;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating error propagation paths from node {NodeId}", errorNodeId);
            throw;
        }
    }
    
    /// <inheritdoc />
    public async Task<RootCauseAnalysisResult> AnalyzeRootCauseAsync(DependencyGraph graph, string errorNodeId)
    {
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(errorNodeId);

        try
        {
            _logger.LogInformation("Analyzing root cause for error node {NodeId}", errorNodeId);
            
            var result = new RootCauseAnalysisResult
            {
                ErrorNodeId = errorNodeId,
                Timestamp = DateTime.UtcNow
            };
            
            // Find all nodes that can reach the error node (potential root causes)
            var potentialRootCauses = new Dictionary<string, double>();
            var visited = new HashSet<string>();
            
            // Reverse graph traversal to find potential root causes
            FindPotentialRootCauses(graph, errorNodeId, potentialRootCauses, visited);
            
            // Order potential root causes by probability
            result.PotentialCauses = potentialRootCauses
                .OrderByDescending(kv => kv.Value)
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            
            // The most likely root cause is the one with highest probability
            if (result.PotentialCauses.Any())
            {
                result.MostLikelyRootCauseId = result.PotentialCauses.First().Key;
                result.RootCauseProbability = result.PotentialCauses.First().Value;
            }
            
            result.Status = AnalysisStatus.Completed;
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing root cause for error node {NodeId}", errorNodeId);
            throw;
        }
    }
    
    /// <inheritdoc />
    public async Task<List<GraphCycle>> FindCyclesAsync(DependencyGraph graph)
    {
        ArgumentNullException.ThrowIfNull(graph);

        try
        {
            _logger.LogInformation("Finding cycles in dependency graph");
            
            var cycles = new List<GraphCycle>();
            var visited = new Dictionary<string, bool>();
            var recStack = new Dictionary<string, bool>();
            
            // Initialize visited and recursion stack for all nodes
            foreach (var node in graph.Nodes)
            {
                visited[node.Key] = false;
                recStack[node.Key] = false;
            }
            
            // Check for cycles starting from each node
            foreach (var node in graph.Nodes)
            {
                if (!visited[node.Key])
                {
                    FindCyclesUtil(graph, node.Key, visited, recStack, new List<string>(), cycles);
                }
            }
            
            return cycles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding cycles in dependency graph");
            throw;
        }
    }
    
    /// <inheritdoc />
    public async Task<Dictionary<string, double>> CalculateCentralityAsync(DependencyGraph graph)
    {
        ArgumentNullException.ThrowIfNull(graph);

        try
        {
            _logger.LogInformation("Calculating centrality scores for dependency graph");
            
            var centrality = new Dictionary<string, double>();
            
            // Calculate degree centrality for each node
            foreach (var node in graph.Nodes)
            {
                // Count incoming and outgoing edges
                int inDegree = graph.Edges.Count(e => e.Value.TargetId == node.Key);
                int outDegree = graph.Edges.Count(e => e.Value.SourceId == node.Key);
                
                // Normalize by maximum possible edges (n-1)
                int nodeCount = graph.Nodes.Count;
                double normalizedDegree = nodeCount <= 1 
                    ? 0 
                    : (inDegree + outDegree) / (double)(2 * (nodeCount - 1));
                
                centrality[node.Key] = normalizedDegree;
            }
            
            return centrality;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating centrality scores for dependency graph");
            throw;
        }
    }

    private double CalculateClusteringCoefficient(DependencyGraph graph)
    {
        if (graph.Nodes.Count <= 1)
            return 0;

        var totalCoefficient = 0.0;
        var nodeCount = 0;

        foreach (var node in graph.Nodes)
        {
            var neighbors = graph.GetNeighbors(node.Key).ToList();
            if (neighbors.Count <= 1)
                continue;

            var possibleEdges = (neighbors.Count * (neighbors.Count - 1)) / 2;
            var actualEdges = 0;

            // Count actual edges between neighbors
            for (var i = 0; i < neighbors.Count; i++)
            {
                for (var j = i + 1; j < neighbors.Count; j++)
                {
                    if (graph.HasEdge(neighbors[i], neighbors[j]) || graph.HasEdge(neighbors[j], neighbors[i]))
                    {
                        actualEdges++;
                    }
                }
            }

            var coefficient = possibleEdges > 0 ? (double)actualEdges / possibleEdges : 0;
            totalCoefficient += coefficient;
            nodeCount++;
        }

        return nodeCount > 0 ? totalCoefficient / nodeCount : 0;
    }

    // Helper methods for the new implementations
    private GraphPath FindLongestPathToNode(DependencyGraph graph, string targetNodeId)
    {
        var distances = new Dictionary<string, int>();
        var predecessors = new Dictionary<string, string>();
        
        // Initialize distances
        foreach (var node in graph.Nodes)
        {
            distances[node.Key] = -1; // -1 represents unreachable
        }
        
        // Use topological sort to find longest path
        var sortedNodes = TopologicalSort(graph);
        
        // Set distance to source as 0
        foreach (var node in graph.Nodes)
        {
            if (!graph.Edges.Any(e => e.Value.TargetId == node.Key))
            {
                distances[node.Key] = 0;
            }
        }
        
        // Process nodes in topological order
        foreach (var nodeId in sortedNodes)
        {
            if (distances[nodeId] != -1)
            {
                var incomingEdges = graph.Edges.Where(e => e.Value.TargetId == nodeId);
                foreach (var edge in incomingEdges)
                {
                    var newDistance = distances[nodeId] + 1;
                    if (distances[edge.Value.SourceId] < newDistance)
                    {
                        distances[edge.Value.SourceId] = newDistance;
                        predecessors[edge.Value.SourceId] = nodeId;
                    }
                }
            }
        }
        
        // Reconstruct the path
        var path = new GraphPath();
        var current = targetNodeId;
        
        if (distances[targetNodeId] == -1)
        {
            return path; // No path to target
        }
        
        while (current != null)
        {
            path.Nodes.Add(current);
            predecessors.TryGetValue(current, out current);
        }
        
        path.Nodes.Reverse(); // Path is reconstructed in reverse order
        
        // Calculate path weight
        path.Weight = path.Nodes.Count - 1; // Weight is the number of edges
        
        return path;
    }
    
    private List<string> TopologicalSort(DependencyGraph graph)
    {
        var visited = new Dictionary<string, bool>();
        var stack = new Stack<string>();
        
        // Initialize visited for all nodes
        foreach (var node in graph.Nodes)
        {
            visited[node.Key] = false;
        }
        
        // Visit each unvisited node
        foreach (var node in graph.Nodes)
        {
            if (!visited[node.Key])
            {
                TopologicalSortUtil(graph, node.Key, visited, stack);
            }
        }
        
        return stack.ToList();
    }
    
    private void TopologicalSortUtil(DependencyGraph graph, string nodeId, Dictionary<string, bool> visited, Stack<string> stack)
    {
        visited[nodeId] = true;
        
        // Visit all outgoing edges
        var outgoingEdges = graph.Edges.Where(e => e.Value.SourceId == nodeId);
        foreach (var edge in outgoingEdges)
        {
            if (!visited[edge.Value.TargetId])
            {
                TopologicalSortUtil(graph, edge.Value.TargetId, visited, stack);
            }
        }
        
        stack.Push(nodeId);
    }
    
    private void FindAllPathsFromNode(DependencyGraph graph, string nodeId, List<string> currentPath, 
        HashSet<string> visited, List<GraphPath> paths)
    {
        visited.Add(nodeId);
        currentPath.Add(nodeId);
        
        // If this is a terminal node, add the path
        if (!graph.Edges.Any(e => e.Value.SourceId == nodeId))
        {
            var newPath = new GraphPath
            {
                Nodes = new List<string>(currentPath),
                Weight = currentPath.Count - 1
            };
            paths.Add(newPath);
        }
        else
        {
            // Continue exploring all outgoing edges
            var outgoingEdges = graph.Edges.Where(e => e.Value.SourceId == nodeId);
            foreach (var edge in outgoingEdges)
            {
                if (!visited.Contains(edge.Value.TargetId))
                {
                    FindAllPathsFromNode(graph, edge.Value.TargetId, currentPath, visited, paths);
                }
            }
        }
        
        // Backtrack
        currentPath.RemoveAt(currentPath.Count - 1);
        visited.Remove(nodeId);
    }
    
    private void FindPotentialRootCauses(DependencyGraph graph, string nodeId, 
        Dictionary<string, double> potentialCauses, HashSet<string> visited)
    {
        visited.Add(nodeId);
        
        // Find all incoming edges
        var incomingEdges = graph.Edges.Where(e => e.Value.TargetId == nodeId);
        foreach (var edge in incomingEdges)
        {
            var sourceNode = graph.Nodes.FirstOrDefault(n => n.Key == edge.Value.SourceId);
            if (sourceNode != null)
            {
                // Add this node as a potential cause with its error probability
                potentialCauses[sourceNode.Key] = sourceNode.Value.ErrorProbability;
                
                // Continue traversal if not visited
                if (!visited.Contains(sourceNode.Key))
                {
                    FindPotentialRootCauses(graph, sourceNode.Key, potentialCauses, visited);
                }
            }
        }
    }
    
    private void FindCyclesUtil(DependencyGraph graph, string nodeId, Dictionary<string, bool> visited,
        Dictionary<string, bool> recStack, List<string> currentPath, List<GraphCycle> cycles)
    {
        visited[nodeId] = true;
        recStack[nodeId] = true;
        currentPath.Add(nodeId);
        
        // Check all neighbors
        var outgoingEdges = graph.Edges.Where(e => e.Value.SourceId == nodeId);
        foreach (var edge in outgoingEdges)
        {
            // If not visited, continue DFS
            if (!visited[edge.Value.TargetId])
            {
                FindCyclesUtil(graph, edge.Value.TargetId, visited, recStack, currentPath, cycles);
            }
            // If already in recursion stack, we found a cycle
            else if (recStack[edge.Value.TargetId])
            {
                // Find the start of the cycle
                int startIdx = currentPath.IndexOf(edge.Value.TargetId);
                if (startIdx != -1)
                {
                    var cycleNodes = currentPath.GetRange(startIdx, currentPath.Count - startIdx);
                    cycleNodes.Add(edge.Value.TargetId); // Complete the cycle
                    
                    var cycle = new GraphCycle
                    {
                        Nodes = cycleNodes,
                        CycleLength = cycleNodes.Count
                    };
                    
                    cycles.Add(cycle);
                }
            }
        }
        
        // Backtrack
        currentPath.RemoveAt(currentPath.Count - 1);
        recStack[nodeId] = false;
    }

    private async Task<Dictionary<string, double>> CalculateGraphMetricsAsync(DependencyGraph graph)
    {
        var metrics = new Dictionary<string, double>
        {
            ["clustering_coefficient"] = CalculateClusteringCoefficient(graph),
            ["centrality"] = CalculateCentrality(graph),
            ["error_probability"] = await CalculateErrorProbabilityAsync(graph),
            ["impact_severity"] = await CalculateImpactSeverityAsync(graph),
            ["error_spread"] = await CalculateErrorSpreadAsync(graph),
            ["component_health"] = await CalculateComponentHealthAsync(graph)
        };

        return metrics;
    }

    private double CalculateCentrality(DependencyGraph graph)
    {
        if (graph.Nodes.Count <= 1)
            return 0;

        var centrality = 0.0;
        var nodeCount = 0;

        foreach (var node in graph.Nodes)
        {
            var shortestPaths = 0;
            var pathsThroughNode = 0;

            foreach (var source in graph.Nodes.Where(n => n.Key != node.Key))
            {
                foreach (var target in graph.Nodes.Where(n => n.Key != node.Key && n.Key != source.Key))
                {
                    var path = graph.GetPath(source.Key, target.Key);
                    if (path != null)
                    {
                        shortestPaths++;
                        if (path.Any(n => n.Key == node.Key))
                        {
                            pathsThroughNode++;
                        }
                    }
                }
            }

            if (shortestPaths > 0)
            {
                centrality += (double)pathsThroughNode / shortestPaths;
                nodeCount++;
            }
        }

        return nodeCount > 0 ? centrality / nodeCount : 0;
    }

    private async Task<double> CalculateErrorProbabilityAsync(DependencyGraph graph)
    {
        if (graph.Nodes.Count == 0)
            return 0;

        var totalProbability = 0.0;
        foreach (var node in graph.Nodes)
        {
            totalProbability += await _errorClassifier.CalculateErrorProbabilityAsync(node.Value);
        }

        return totalProbability / graph.Nodes.Count;
    }

    private async Task<double> CalculateImpactSeverityAsync(DependencyGraph graph)
    {
        if (graph.Nodes.Count == 0)
            return 0;

        var totalSeverity = 0.0;
        foreach (var node in graph.Nodes)
        {
            var errorContext = new ErrorContext(
                _errorFactory.CreateError(
                    type: node.Value.NodeType,
                    message: "Node error analysis",
                    source: node.Key,
                    stackTrace: string.Empty
                ),
                environment: null,
                timestamp: DateTime.UtcNow
            );

            errorContext.ComponentId = node.Key;
            errorContext.ErrorType = node.Value.NodeType;
            errorContext.ServiceName = graph.Metadata.TryGetValue("ServiceName", out var svc) ? svc?.ToString() : string.Empty;
            
            errorContext.AddMetadata("NodeId", node.Key);
            errorContext.AddMetadata("NodeType", node.Value.NodeType);
            var impactResults = await _impactAnalyzer.AnalyzeImpactAsync(errorContext, graph);
            var impact = impactResults?.FirstOrDefault();
            if (impact != null)
            {
                totalSeverity += impact.ImpactMetrics.GetValueOrDefault("severity", 0);
            }
        }

        return totalSeverity / graph.Nodes.Count;
    }

    private async Task<double> CalculateErrorSpreadAsync(DependencyGraph graph)
    {
        if (graph.Nodes.Count == 0)
            return 0;

        var errorNodes = graph.Nodes.Where(n => n.Value.ErrorProbability > 0.5).ToList();
        if (!errorNodes.Any())
            return 0;

        var totalSpread = 0.0;
        foreach (var errorNode in errorNodes)
        {
            var errorContext = new ErrorContext(
                _errorFactory.CreateError(
                    type: errorNode.Value.NodeType,
                    message: "Error spread analysis",
                    source: errorNode.Key,
                    stackTrace: string.Empty
                ),
                environment: null,
                timestamp: DateTime.UtcNow
            );

            errorContext.ComponentId = errorNode.Key;
            errorContext.AddMetadata("NodeId", errorNode.Key);
            errorContext.AddMetadata("NodeType", errorNode.Value.NodeType);
            errorContext.AddMetadata("ErrorProbability", errorNode.Value.ErrorProbability);

            var impactResults = await _impactAnalyzer.AnalyzeImpactAsync(errorContext, graph);
            var impact = impactResults?.FirstOrDefault();
            if (impact != null && impact.AffectedNodes != null)
            {
                totalSpread += (double)impact.AffectedNodes.Count / graph.Nodes.Count;
            }
        }

        return totalSpread / (double)errorNodes.Count;
    }

    private async Task<double> CalculateComponentHealthAsync(DependencyGraph graph)
    {
        if (graph.Nodes.Count == 0)
            return 0;

        var totalHealth = 0.0;
        foreach (var node in graph.Nodes)
        {
            var health = 1.0 - node.Value.ErrorProbability;
            if (health < 0.0) health = 0.0;
            if (health > 1.0) health = 1.0;
            totalHealth += health;
        }

        return totalHealth / graph.Nodes.Count;
    }
} 
