using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Graph;
using RuntimeErrorSage.Application.Services.Interfaces;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Services;

/// <summary>
/// Service for analyzing the impact of errors on system components.
/// </summary>
public class ImpactAnalyzer : IImpactAnalyzer
{
    private readonly ILogger<ImpactAnalyzer> _logger;

    public ImpactAnalyzer(ILogger<ImpactAnalyzer> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Collection<RuntimeErrorSage.Application.Models.Graph.ImpactAnalysisResult>> AnalyzeImpactAsync(ErrorContext context, DependencyGraph graph)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(graph);

        try
        {
            _logger.LogInformation("Analyzing impact for error {ErrorId}", context.ErrorId);

            var results = new Collection<RuntimeErrorSage.Application.Models.Graph.ImpactAnalysisResult>();

            // Start from the error source node
            var startNodeId = context.ErrorSource;
            if (string.IsNullOrEmpty(startNodeId) || !graph.Nodes.ContainsKey(startNodeId))
            {
                _logger.LogWarning("No valid error source node found for error {ErrorId}", context.ErrorId);
                return results;
            }

            // Analyze impact on each node
            foreach (var node in graph.Nodes.Values)
            {
                var result = await AnalyzeNodeImpactAsync(node, graph, startNodeId);
                if (result != null)
                {
                    results.Add(result);
                }
            }

            // Sort results by severity
            results = results.OrderByDescending(r => r.Severity).ToList();

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing impact for error {ErrorId}", context.ErrorId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> ValidateConfigurationAsync()
    {
        try
        {
            // Add any configuration validation logic here
            await Task.CompletedTask;
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating impact analyzer configuration");
            return false;
        }
    }

    private async Task<RuntimeErrorSage.Application.Models.Graph.ImpactAnalysisResult> AnalyzeNodeImpactAsync(GraphNode node, DependencyGraph graph, string startNodeId)
    {
        try
        {
            var result = new RuntimeErrorSage.Application.Models.Graph.ImpactAnalysisResult
            {
                ErrorId = graph.Metadata.TryGetValue("ErrorId", out var errorId) ? errorId?.ToString() : string.Empty,
                ComponentId = node.Id,
                ComponentName = node.Name,
                ComponentType = node.Type,
                Timestamp = DateTime.UtcNow
            };

            // Calculate direct dependencies
            var directDependencies = graph.Edges
                .Where(e => e.Source.Id == node.Id)
                .Select(e => new DependencyNode
                {
                    Id = e.Target.Id,
                    Type = e.Target.Type,
                    Weight = e.Weight
                })
                .ToList();

            result.DirectDependencies = directDependencies;

            // Calculate indirect dependencies
            var indirectDependencies = new Collection<DependencyNode>();
            var visited = new HashSet<string> { node.Id };
            var queue = new Queue<string>();
            foreach (var dep in directDependencies)
            {
                queue.Enqueue(dep.Id);
            }

            while (queue.Count > 0)
            {
                var currentId = queue.Dequeue();
                if (visited.Contains(currentId))
                    continue;

                visited.Add(currentId);
                var currentEdges = graph.Edges.Where(e => e.Source.Id == currentId);
                foreach (var edge in currentEdges)
                {
                    if (!visited.Contains(edge.Target.Id))
                    {
                        indirectDependencies.Add(new DependencyNode
                        {
                            Id = edge.Target.Id,
                            Type = edge.Target.Type,
                            Weight = edge.Weight
                        });
                        queue.Enqueue(edge.Target.Id);
                    }
                }
            }

            result.IndirectDependencies = indirectDependencies;

            // Calculate blast radius
            result.BlastRadius = directDependencies.Count + indirectDependencies.Count;

            // Calculate severity based on blast radius and node type
            result.Severity = CalculateSeverity(result.BlastRadius, node.Type);
            result.Scope = CalculateScope(result.BlastRadius);

            // Calculate confidence based on available data
            result.Confidence = CalculateConfidence(node, graph);

            await Task.CompletedTask;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing impact for node {NodeId}", node.Id);
            return null;
        }
    }

    private SeverityLevel CalculateSeverity(int blastRadius, GraphNodeType nodeType)
    {
        if (blastRadius > 10 || nodeType == GraphNodeType.Critical)
            return SeverityLevel.Critical;
        if (blastRadius > 5 || nodeType == GraphNodeType.High)
            return SeverityLevel.High;
        if (blastRadius > 2 || nodeType == GraphNodeType.Medium)
            return SeverityLevel.Medium;
        return SeverityLevel.Low;
    }

    private ImpactScope CalculateScope(int blastRadius)
    {
        if (blastRadius > 10)
            return ImpactScope.System;
        if (blastRadius > 5)
            return ImpactScope.Service;
        if (blastRadius > 2)
            return ImpactScope.Component;
        return ImpactScope.Local;
    }

    private double CalculateConfidence(GraphNode node, DependencyGraph graph)
    {
        var confidence = 0.0;

        // Base confidence on node type
        switch (node.Type)
        {
            case GraphNodeType.Component:
                confidence += 0.3;
                break;
            case GraphNodeType.Error:
                confidence += 0.5;
                break;
            default:
                confidence += 0.2;
                break;
        }

        // Adjust confidence based on available metadata
        if (node.Metadata != null && node.Metadata.Count > 0)
            confidence += 0.2;

        // Adjust confidence based on graph completeness
        var totalPossibleEdges = graph.Nodes.Count * (graph.Nodes.Count - 1);
        var actualEdges = graph.Edges.Count;
        var graphCompleteness = totalPossibleEdges > 0 ? (double)actualEdges / totalPossibleEdges : 0;
        confidence += graphCompleteness * 0.3;

        return Math.Min(confidence, 1.0);
    }
} 






