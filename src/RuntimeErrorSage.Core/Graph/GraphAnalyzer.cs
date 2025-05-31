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
using RuntimeErrorSage.Core.Graph;
using RuntimeErrorSage.Application.Extensions;

// Use alias to avoid ambiguity
using GraphComponentDependency = RuntimeErrorSage.Domain.Models.Graph.ComponentDependency;
using ErrorComponentDependency = RuntimeErrorSage.Domain.Models.Error.ComponentDependency;

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
                
                if (currentNode.Key == null)
                    continue;

                // Calculate impact probability for this node
                double impactProbability = 0.8; // Default value
                result.AffectedNodesMap[currentNodeId] = impactProbability;

                // Find outgoing edges
                var outgoingEdges = graph.Edges.Where(e => e.Value.SourceId == currentNodeId);

                foreach (var edge in outgoingEdges)
                {
                    string targetId = edge.Value.TargetId;
                    
                    if (!visited.Contains(targetId))
                    {
                        visited.Add(targetId);
                        queue.Enqueue(targetId);
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
                // Convert GraphNode to DependencyNode if needed
                var dependencyNode = node.Value.ToDependencyNode();
                
                // Check if this node meets any of the criteria
                var incomingEdges = graph.Edges.Where(e => e.Value.TargetId == node.Key);
                var outgoingEdges = graph.Edges.Where(e => e.Value.SourceId == node.Key);
                
                // Add to high-risk nodes if critical or high error probability
                if (dependencyNode.IsCritical || dependencyNode.ErrorProbability > threshold ||
                    incomingEdges.Any(e => e.Value.Weight > threshold) ||
                    outgoingEdges.Any(e => e.Value.Weight > threshold))
                {
                    highRiskNodes.Add(dependencyNode);
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
            var currentPath = new List<string>();
            
            FindAllPathsFromNode(graph, errorNodeId, currentPath, visited, propagationPaths);
            
            return propagationPaths;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating error propagation paths");
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
                Timestamp = DateTime.UtcNow,
                PotentialCauses = new Dictionary<string, double>()
            };
            
            // Find potential root causes by traversing the graph backwards
            var visited = new HashSet<string>();
            // Create a dictionary to store potential causes
            var potentialCausesDict = new Dictionary<string, double>();
            FindPotentialRootCauses(graph, errorNodeId, potentialCausesDict, visited);
            
            // Set the potential causes
            result.PotentialCauses = potentialCausesDict;
            
            // Convert dictionary to list of nodes for RootCauseNodes property
            var rootCauseNodes = new List<GraphNode>();
            foreach (var causeEntry in potentialCausesDict.OrderByDescending(kv => kv.Value))
            {
                // Try to find the node in the graph nodes
                if (graph.Nodes.TryGetValue(causeEntry.Key, out var node))
                {
                    // Add the node to the root cause nodes list
                    rootCauseNodes.Add(node);
                }
            }
            
            // Set the root cause nodes
            result.RootCauseNodes = rootCauseNodes;
            
            if (result.PotentialCauses.Count > 0)
            {
                var mostLikelyRootCause = result.PotentialCauses.First();
                result.MostLikelyRootCauseId = mostLikelyRootCause.Key;
                result.RootCauseProbability = mostLikelyRootCause.Value;
                
                // Get the node data for the most likely root cause
                if (graph.Nodes.TryGetValue(result.MostLikelyRootCauseId, out var rootCauseNode))
                {
                    // Set properties based on available properties in rootCauseNode
                    result.RootCauseNode = rootCauseNode;
                    // Set any additional properties if needed
                }
            }
            
            result.Status = AnalysisStatus.Completed;
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing root cause");
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
            
            // Initialize visited and recursion stack
            foreach (var node in graph.Nodes)
            {
                visited[node.Key] = false;
                recStack[node.Key] = false;
            }
            
            // DFS to find cycles
            foreach (var node in graph.Nodes)
            {
                if (!visited[node.Key])
                {
                    var currentPath = new List<string>();
                    FindCyclesUtil(graph, node.Key, visited, recStack, currentPath, cycles);
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
            _logger.LogInformation("Calculating centrality for dependency graph");
            
            var centrality = new Dictionary<string, double>();
            
            foreach (var node in graph.Nodes)
            {
                // Calculate both in-degree and out-degree
                int inDegree = graph.Edges.Count(e => e.Value.TargetId == node.Key);
                int outDegree = graph.Edges.Count(e => e.Value.SourceId == node.Key);
                
                // Simple centrality measure is the sum of in-degree and out-degree
                centrality[node.Key] = inDegree + outDegree;
            }
            
            // Normalize centrality values
            double max = centrality.Values.Max();
            foreach (var node in graph.Nodes)
            {
                centrality[node.Key] /= max;
            }
            
            return centrality;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating centrality");
            throw;
        }
    }
    
    /// <summary>
    /// Calculates the clustering coefficient of the graph.
    /// </summary>
    /// <param name="graph">The dependency graph.</param>
    /// <returns>The clustering coefficient.</returns>
    private double CalculateClusteringCoefficient(DependencyGraph graph)
    {
        double sum = 0;
        int count = 0;
        
        foreach (var node in graph.Nodes)
        {
            // Get neighbors (both incoming and outgoing)
            var neighbors = new HashSet<string>();
            
            foreach (var edge in graph.Edges)
            {
                if (edge.Value.SourceId == node.Key)
                {
                    neighbors.Add(edge.Value.TargetId);
                }
                else if (edge.Value.TargetId == node.Key)
                {
                    neighbors.Add(edge.Value.SourceId);
                }
            }
            
            if (neighbors.Count < 2)
            {
                continue; // Skip nodes with fewer than 2 neighbors
            }
            
            // Count edges between neighbors
            int edgesBetweenNeighbors = 0;
            foreach (var neighbor1 in neighbors)
            {
                foreach (var neighbor2 in neighbors)
                {
                    if (neighbor1 != neighbor2 && graph.HasEdge(neighbor1, neighbor2))
                    {
                        edgesBetweenNeighbors++;
                    }
                }
            }
            
            double possibleEdges = neighbors.Count * (neighbors.Count - 1);
            double nodeCoefficient = possibleEdges > 0 ? edgesBetweenNeighbors / possibleEdges : 0;
            
            sum += nodeCoefficient;
            count++;
        }
        
        return count > 0 ? sum / count : 0;
    }
    
    /// <summary>
    /// Finds the longest path to a node in the graph.
    /// </summary>
    /// <param name="graph">The dependency graph.</param>
    /// <param name="targetNodeId">The target node ID.</param>
    /// <returns>The longest path to the target node.</returns>
    private GraphPath FindLongestPathToNode(DependencyGraph graph, string targetNodeId)
    {
        // Topologically sort the graph
        var sortedNodes = TopologicalSort(graph);
        
        // Initialize distances
        var distances = new Dictionary<string, int>();
        var predecessors = new Dictionary<string, string>();
        
        foreach (var nodeId in sortedNodes)
        {
            distances[nodeId] = int.MinValue;
            predecessors[nodeId] = null;
        }
        
        // Find entry nodes (nodes with no incoming edges)
        var entryNodes = graph.Nodes
            .Where(n => !graph.Edges.Any(e => e.Value.TargetId == n.Key))
            .Select(n => n.Key)
            .ToList();
        
        foreach (var entryNodeId in entryNodes)
        {
            distances[entryNodeId] = 0;
        }
        
        // Process nodes in topological order
        foreach (var nodeId in sortedNodes)
        {
            // Skip if this node is unreachable
            if (distances[nodeId] == int.MinValue)
                continue;
            
            // Process outgoing edges
            var outgoingEdges = graph.Edges.Where(e => e.Value.SourceId == nodeId);
            
            foreach (var edge in outgoingEdges)
            {
                string targetId = edge.Value.TargetId;
                
                if (distances[nodeId] + 1 > distances[targetId])
                {
                    distances[targetId] = distances[nodeId] + 1;
                    predecessors[targetId] = nodeId;
                }
            }
        }
        
        // Build the path from the target node back to an entry node
        var path = new GraphPath();
        
        if (distances[targetNodeId] == int.MinValue)
        {
            // No path to the target node
            return path;
        }
        
        // Reconstruct the path
        var pathNodes = new List<string>();
        string currentNodeId = targetNodeId;
        
        while (currentNodeId != null)
        {
            pathNodes.Add(currentNodeId);
            currentNodeId = predecessors[currentNodeId];
        }
        
        pathNodes.Reverse();
        
        // Create graph nodes for the path
        var pathGraphNodes = new List<GraphNode>();
        foreach (var nodeId in pathNodes)
        {
            var node = graph.Nodes.FirstOrDefault(n => n.Key == nodeId).Value;
            if (node != null)
            {
                pathGraphNodes.Add(node);
            }
        }
        
        // Create a new GraphPath with the correct length since Length is read-only
        path = new GraphPath
        {
            Nodes = pathGraphNodes,
            IsCycle = false
        };
        
        return path;
    }
    
    /// <summary>
    /// Performs topological sorting of the graph.
    /// </summary>
    /// <param name="graph">The dependency graph.</param>
    /// <returns>A list of node IDs in topological order.</returns>
    private List<string> TopologicalSort(DependencyGraph graph)
    {
        var visited = new Dictionary<string, bool>();
        var stack = new Stack<string>();
        
        foreach (var node in graph.Nodes)
        {
            visited[node.Key] = false;
        }
        
        foreach (var node in graph.Nodes)
        {
            if (!visited[node.Key])
            {
                TopologicalSortUtil(graph, node.Key, visited, stack);
            }
        }
        
        return stack.ToList();
    }
    
    /// <summary>
    /// Utility method for topological sorting.
    /// </summary>
    private void TopologicalSortUtil(DependencyGraph graph, string nodeId, Dictionary<string, bool> visited, Stack<string> stack)
    {
        visited[nodeId] = true;
        
        // Process all adjacent vertices
        var outgoingEdges = graph.Edges.Where(e => e.Value.SourceId == nodeId);
        
        foreach (var edge in outgoingEdges)
        {
            string targetId = edge.Value.TargetId;
            
            if (!visited.ContainsKey(targetId))
            {
                // Handle case where target node is not in the graph
                continue;
            }
            
            if (!visited[targetId])
            {
                TopologicalSortUtil(graph, targetId, visited, stack);
            }
        }
        
        // Push current node to stack
        stack.Push(nodeId);
    }
    
    /// <summary>
    /// Finds all paths from a node using DFS.
    /// </summary>
    private void FindAllPathsFromNode(DependencyGraph graph, string nodeId, List<string> currentPath, 
        HashSet<string> visited, List<GraphPath> paths)
    {
        visited.Add(nodeId);
        currentPath.Add(nodeId);
        
        // Get outgoing edges
        var outgoingEdges = graph.Edges.Where(e => 
            e.Value.SourceId == nodeId);
        
        foreach (var edge in outgoingEdges)
        {
            string targetId = edge.Value.TargetId;
            
            if (!visited.Contains(targetId))
            {
                FindAllPathsFromNode(graph, targetId, currentPath, visited, paths);
            }
            else if (currentPath.Contains(targetId))
            {
                // We found a cycle, create a path for it
                int startIndex = currentPath.IndexOf(targetId);
                var cycleNodes = currentPath.Skip(startIndex).ToList();
                
                // Convert to GraphNode objects
                var graphNodes = new List<GraphNode>();
                foreach (var id in cycleNodes)
                {
                    var node = graph.Nodes.FirstOrDefault(n => n.Key == id).Value;
                    if (node != null)
                    {
                        graphNodes.Add(node);
                    }
                }
                
                var cyclePath = new GraphPath
                {
                    Nodes = graphNodes,
                    IsCycle = true
                };
                
                paths.Add(cyclePath);
            }
        }
        
        // If this is a terminal node (no outgoing edges) and we have at least 2 nodes in the path,
        // add the path to the result
        if (!outgoingEdges.Any() && currentPath.Count > 1)
        {
            var pathNodes = new List<GraphNode>();
            foreach (var id in currentPath)
            {
                var node = graph.Nodes.FirstOrDefault(n => n.Key == id).Value;
                if (node != null)
                {
                    pathNodes.Add(node);
                }
            }
            
            var path = new GraphPath
            {
                Nodes = pathNodes,
                IsCycle = false
            };
            
            paths.Add(path);
        }
        
        // Remove the current node from path and visited for backtracking
        currentPath.RemoveAt(currentPath.Count - 1);
        visited.Remove(nodeId);
    }
    
    /// <summary>
    /// Finds potential root causes by traversing the graph backwards.
    /// </summary>
    private void FindPotentialRootCauses(
        DependencyGraph graph, 
        string errorNodeId, 
        Dictionary<string, double> potentialCauses, 
        HashSet<string> visited)
    {
        if (string.IsNullOrEmpty(errorNodeId) || visited.Contains(errorNodeId)) return;
        visited.Add(errorNodeId);

        // Find inbound connections to this node
        foreach (var edge in graph.Edges)
        {
            if (edge.Value.TargetId == errorNodeId)
            {
                var sourceId = edge.Value.SourceId;
                
                // Check if the node is an error source
                if (graph.Nodes.TryGetValue(sourceId, out var sourceNode) && sourceNode.ErrorProbability > 0)
                {
                    potentialCauses[sourceId] = sourceNode.ErrorProbability;
                }
                
                // Recursively check this node's dependencies
                FindPotentialRootCauses(graph, sourceId, potentialCauses, visited);
            }
        }
    }
    
    /// <summary>
    /// Utility method to find cycles in the graph.
    /// </summary>
    private void FindCyclesUtil(DependencyGraph graph, string nodeId, Dictionary<string, bool> visited,
        Dictionary<string, bool> recStack, List<string> currentPath, List<GraphCycle> cycles)
    {
        // Mark the current node as visited and part of recursion stack
        visited[nodeId] = true;
        recStack[nodeId] = true;
        currentPath.Add(nodeId);
        
        // Process all adjacent vertices
        var outgoingEdges = graph.Edges.Where(e => e.Value.SourceId == nodeId);
        
        foreach (var edge in outgoingEdges)
        {
            string targetId = edge.Value.TargetId;
            
            // If not visited, recursive call
            if (!visited.ContainsKey(targetId))
            {
                // Skip if target node is not in the graph
                continue;
            }
            else if (!visited[targetId])
            {
                FindCyclesUtil(graph, targetId, visited, recStack, currentPath, cycles);
            }
            // If visited and in recursion stack, we found a cycle
            else if (recStack[targetId])
            {
                // Extract the cycle
                int startIndex = currentPath.IndexOf(targetId);
                var cycleNodeIds = currentPath.Skip(startIndex).ToList();
                cycleNodeIds.Add(targetId); // Complete the cycle
                
                // Convert node IDs to GraphNode objects
                var graphNodes = new List<GraphNode>();
                foreach (var id in cycleNodeIds)
                {
                    if (graph.Nodes.TryGetValue(id, out var node))
                    {
                        graphNodes.Add(node);
                    }
                }
                
                // Create a new GraphCycle
                var cycle = new GraphCycle
                {
                    Nodes = graphNodes
                    // CycleLength is computed automatically from Nodes.Count
                };
                
                // Only add if not already present
                if (!cycles.Any(c => c.Nodes.SequenceEqual(cycle.Nodes)))
                {
                    cycles.Add(cycle);
                }
            }
        }
        
        // Remove node from recursion stack and path
        recStack[nodeId] = false;
        currentPath.RemoveAt(currentPath.Count - 1);
    }
    
    /// <inheritdoc />
    public async Task<ImpactAnalysisResult> CalculateImpactAsync(DependencyGraph graph, string nodeId)
    {
        var result = new ImpactAnalysisResult
        {
            StartNodeId = nodeId,
            AffectedNodesMap = new Dictionary<string, double>()
        };
        
        // Use the graph to calculate which nodes would be affected and with what probability
        var paths = await CalculateErrorPropagationPathsAsync(graph, nodeId);
        
        // A node that appears in more paths has higher impact
        var nodeFrequency = new Dictionary<string, int>();
        
        foreach (var path in paths)
        {
            foreach (var node in path.Nodes)
            {
                if (!nodeFrequency.ContainsKey(node.Id))
                {
                    nodeFrequency[node.Id] = 0;
                }
                
                nodeFrequency[node.Id]++;
            }
        }
        
        // Convert frequency to probability
        int maxFrequency = nodeFrequency.Values.Any() ? nodeFrequency.Values.Max() : 0;
        
        foreach (var entry in nodeFrequency)
        {
            double probability = maxFrequency > 0 ? (double)entry.Value / maxFrequency : 0;
            result.AffectedNodesMap[entry.Key] = probability;
        }
        
        result.TotalImpactScore = result.AffectedNodesMap.Values.Sum();
        
        return result;
    }
    
    /// <summary>
    /// Calculates graph metrics for the dependency graph.
    /// </summary>
    private async Task<Dictionary<string, double>> CalculateGraphMetricsAsync(DependencyGraph graph)
    {
        var metrics = new Dictionary<string, double>();
        
        // Node count
        metrics["NodeCount"] = graph.Nodes.Count;
        
        // Edge count
        metrics["EdgeCount"] = graph.Edges.Count;
        
        // Average node degree
        metrics["AverageDegree"] = CalculateCentrality(graph);
        
        // Error probability
        metrics["ErrorProbability"] = await CalculateErrorProbabilityAsync(graph);
        
        // Impact severity
        metrics["ImpactSeverity"] = await CalculateImpactSeverityAsync(graph);
        
        // Clustering coefficient
        metrics["ClusteringCoefficient"] = CalculateClusteringCoefficient(graph);
        
        return metrics;
    }
    
    /// <summary>
    /// Calculates the centrality of the graph.
    /// </summary>
    private double CalculateCentrality(DependencyGraph graph)
    {
        double totalDegree = 0;
        
        foreach (var node in graph.Nodes)
        {
            // Count both incoming and outgoing edges
            int inDegree = graph.Edges.Count(e => e.Value.TargetId == node.Key);
            int outDegree = graph.Edges.Count(e => e.Value.SourceId == node.Key);
            
            totalDegree += inDegree + outDegree;
        }
        
        return graph.Nodes.Count > 0 ? totalDegree / (graph.Nodes.Count * 2) : 0;
    }
    
    /// <summary>
    /// Analyzes the graph to determine error probability.
    /// </summary>
    private async Task<double> CalculateErrorProbabilityAsync(DependencyGraph graph)
    {
        // Calculate average error probability across all nodes
        double totalProbability = 0;
        int nodeCount = 0;
        
        foreach (var node in graph.Nodes)
        {
            if (node.Value.NodeType == GraphNodeType.Service || 
                node.Value.NodeType == GraphNodeType.Component ||
                node.Value.NodeType == GraphNodeType.Error)
            {
                totalProbability += node.Value.ErrorProbability;
                nodeCount++;
            }
        }
        
        return nodeCount > 0 ? totalProbability / nodeCount : 0;
    }
    
    /// <summary>
    /// Analyzes the graph to determine impact severity.
    /// </summary>
    private async Task<double> CalculateImpactSeverityAsync(DependencyGraph graph)
    {
        // Create a simple error context for analysis
        var error = new RuntimeError(
            message: "Synthetic error for impact analysis",
            errorType: "SyntheticError",
            source: "GraphAnalyzer",
            stackTrace: string.Empty
        );
        
        var context = new ErrorContext(
            error: error,
            context: "Impact analysis"
        );
        
        double totalSeverity = 0;
        
        // Find critical nodes (either marked as critical or with high error probability)
        var criticalNodes = graph.Nodes
            .Where(n => n.Value.IsCritical || n.Value.ErrorProbability > 0.7)
            .ToList();
        
        foreach (var node in criticalNodes)
        {
            // Calculate impact from this node
            var impactResult = await CalculateImpactAsync(graph, node.Key);
            totalSeverity += impactResult.TotalImpactScore;
        }
        
        return criticalNodes.Count > 0 ? totalSeverity / criticalNodes.Count : 0;
    }
    
    /// <summary>
    /// Analyzes how errors would spread through the graph.
    /// </summary>
    private async Task<double> CalculateErrorSpreadAsync(DependencyGraph graph)
    {
        // Find all nodes with NodeType = Error
        var errorNodes = graph.Nodes
            .Where(n => n.Value.NodeType == GraphNodeType.Error)
            .ToList();
        
        if (!errorNodes.Any())
        {
            return 0;
        }
        
        double totalSpread = 0;
        
        foreach (var errorNode in errorNodes)
        {
            // Calculate impact from this error node
            var impactResult = await AnalyzeImpactAsync(graph, errorNode.Key);
            totalSpread += impactResult.AffectedNodesMap.Count;
        }
        
        // Normalize by the total number of nodes
        return totalSpread / (errorNodes.Count * graph.Nodes.Count);
    }
    
    /// <summary>
    /// Calculates the overall health of components in the graph.
    /// </summary>
    private async Task<double> CalculateComponentHealthAsync(DependencyGraph graph)
    {
        var componentNodes = graph.Nodes
            .Where(n => n.Value.NodeType == GraphNodeType.Component)
            .ToList();
        
        if (!componentNodes.Any())
        {
            return 1.0; // Default to perfect health if no components
        }
        
        double totalHealth = 0;
        
        foreach (var component in componentNodes)
        {
            // Health is inverse of error probability
            totalHealth += 1.0 - component.Value.ErrorProbability;
        }
        
        return totalHealth / componentNodes.Count;
    }
    
    /// <summary>
    /// Determines if the graph has a cycle.
    /// </summary>
    private bool HasCycle(DependencyGraph graph, Dictionary<string, bool> visited, 
        Dictionary<string, bool> recursionStack, string currentNode, List<string> cycle)
    {
        if (!visited.ContainsKey(currentNode))
        {
            return false;
        }
        
        // Mark current node as visited and add to recursion stack
        visited[currentNode] = true;
        recursionStack[currentNode] = true;
        
        // Get all adjacent nodes
        var adjacentNodes = graph.Edges
            .Where(e => e.Value.SourceId == currentNode)
            .Select(e => e.Value.TargetId)
            .ToList();
        
        foreach (var adjacent in adjacentNodes)
        {
            if (!visited.ContainsKey(adjacent))
            {
                continue;
            }
            
            // If adjacent node is not visited, recursively check it
            if (!visited[adjacent])
            {
                cycle.Add(currentNode);
                if (HasCycle(graph, visited, recursionStack, adjacent, cycle))
                {
                    return true;
                }
                cycle.Remove(currentNode);
            }
            // If adjacent node is visited and in recursion stack, we found a cycle
            else if (recursionStack[adjacent])
            {
                cycle.Add(currentNode);
                cycle.Add(adjacent);
                return true;
            }
        }
        
        // Remove current node from recursion stack
        recursionStack[currentNode] = false;
        return false;
    }
} 
