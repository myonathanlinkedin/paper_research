using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Graph.Interfaces;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Services.Interfaces;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Graph;

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

    public GraphAnalyzer(
        ILogger<GraphAnalyzer> logger,
        IGraphBuilder graphBuilder,
        IImpactAnalyzer impactAnalyzer,
        IErrorRelationshipAnalyzer relationshipAnalyzer,
        IErrorClassifier errorClassifier)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
        _impactAnalyzer = impactAnalyzer ?? throw new ArgumentNullException(nameof(impactAnalyzer));
        _relationshipAnalyzer = relationshipAnalyzer ?? throw new ArgumentNullException(nameof(relationshipAnalyzer));
        _errorClassifier = errorClassifier ?? throw new ArgumentNullException(nameof(errorClassifier));
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
            result.ImpactResults = await _impactAnalyzer.AnalyzeImpactAsync(context, result.DependencyGraph);

            // Find related errors
            result.RelatedErrors = await _relationshipAnalyzer.FindRelatedErrorsAsync(context, result.DependencyGraph);

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
    public async Task<ImpactAnalysisResult> AnalyzeImpactAsync(ErrorContext context, DependencyGraph graph)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(graph);

        try
        {
            _logger.LogInformation("Analyzing impact for error {ErrorId}", context.ErrorId);
            var results = await _impactAnalyzer.AnalyzeImpactAsync(context, graph);
            return results.FirstOrDefault() ?? new ImpactAnalysisResult
            {
                AnalysisId = Guid.NewGuid().ToString(),
                ComponentId = context.ComponentId,
                Timestamp = DateTime.UtcNow,
                IsValid = false,
                Description = "No impact analysis results available"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing impact for error {ErrorId}", context.ErrorId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RelatedError>> FindRelatedErrorsAsync(ErrorContext context, DependencyGraph graph)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(graph);

        try
        {
            _logger.LogInformation("Finding related errors for error {ErrorId}", context.ErrorId);
            return await _relationshipAnalyzer.FindRelatedErrorsAsync(context, graph);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding related errors for error {ErrorId}", context.ErrorId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> ValidateConfigurationAsync()
    {
        try
        {
            _logger.LogInformation("Validating graph analyzer configuration");

            // Validate graph builder configuration
            var graphBuilderValid = await _graphBuilder.ValidateConfigurationAsync();
            if (!graphBuilderValid)
            {
                _logger.LogWarning("Graph builder configuration validation failed");
                return false;
            }

            // Validate impact analyzer configuration
            var impactAnalyzerValid = await _impactAnalyzer.ValidateConfigurationAsync();
            if (!impactAnalyzerValid)
            {
                _logger.LogWarning("Impact analyzer configuration validation failed");
                return false;
            }

            // Validate relationship analyzer configuration
            var relationshipAnalyzerValid = await _relationshipAnalyzer.ValidateConfigurationAsync();
            if (!relationshipAnalyzerValid)
            {
                _logger.LogWarning("Relationship analyzer configuration validation failed");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating graph analyzer configuration");
            return false;
        }
    }

    private async Task<Dictionary<string, double>> CalculateGraphMetricsAsync(DependencyGraph graph)
    {
        var metrics = new Dictionary<string, double>();

        // Calculate basic graph metrics
        metrics["node_count"] = graph.Nodes.Count;
        metrics["edge_count"] = graph.Edges.Count;
        metrics["density"] = CalculateGraphDensity(graph);
        metrics["average_degree"] = CalculateAverageDegree(graph);
        metrics["max_degree"] = CalculateMaxDegree(graph);
        metrics["average_path_length"] = CalculateAveragePathLength(graph);
        metrics["clustering_coefficient"] = CalculateClusteringCoefficient(graph);
        metrics["centrality"] = CalculateCentrality(graph);

        // Calculate error-specific metrics
        metrics["error_probability"] = await CalculateErrorProbabilityAsync(graph);
        metrics["impact_severity"] = await CalculateImpactSeverityAsync(graph);
        metrics["error_spread"] = await CalculateErrorSpreadAsync(graph);

        // Calculate component health metrics
        metrics["component_health"] = await CalculateComponentHealthAsync(graph);
        metrics["system_reliability"] = await CalculateSystemReliabilityAsync(graph);

        return metrics;
    }

    private double CalculateGraphDensity(DependencyGraph graph)
    {
        if (graph.Nodes.Count <= 1) return 0;
        return (2.0 * graph.Edges.Count) / (graph.Nodes.Count * (graph.Nodes.Count - 1));
    }

    private double CalculateAverageDegree(DependencyGraph graph)
    {
        if (graph.Nodes.Count == 0) return 0;
        return (2.0 * graph.Edges.Count) / graph.Nodes.Count;
    }

    private double CalculateMaxDegree(DependencyGraph graph)
    {
        if (graph.Nodes.Count == 0) return 0;
        return graph.Nodes.Max(node => 
            graph.Edges.Count(e => e.SourceId == node.Id || e.TargetId == node.Id));
    }

    private double CalculateAveragePathLength(DependencyGraph graph)
    {
        if (graph.Nodes.Count <= 1) return 0;

        var totalLength = 0.0;
        var pathCount = 0;

        foreach (var source in graph.Nodes)
        {
            foreach (var target in graph.Nodes.Where(n => n.Id != source.Id))
            {
                var path = graph.GetPath(source.Id, target.Id);
                if (path != null)
                {
                    totalLength += path.Count - 1;
                    pathCount++;
                }
            }
        }

        return pathCount > 0 ? totalLength / pathCount : 0;
    }

    private double CalculateClusteringCoefficient(DependencyGraph graph)
    {
        if (graph.Nodes.Count <= 2) return 0;

        var totalCoefficient = 0.0;
        var nodeCount = 0;

        foreach (var node in graph.Nodes)
        {
            var neighbors = graph.GetNeighbors(node.Id);
            if (neighbors.Count <= 1) continue;

            var possibleEdges = (neighbors.Count * (neighbors.Count - 1)) / 2;
            var actualEdges = 0;

            for (int i = 0; i < neighbors.Count; i++)
            {
                for (int j = i + 1; j < neighbors.Count; j++)
                {
                    if (graph.Edges.Any(e => 
                        (e.SourceId == neighbors[i].Id && e.TargetId == neighbors[j].Id) ||
                        (e.SourceId == neighbors[j].Id && e.TargetId == neighbors[i].Id)))
                    {
                        actualEdges++;
                    }
                }
            }

            if (possibleEdges > 0)
            {
                totalCoefficient += (double)actualEdges / possibleEdges;
                nodeCount++;
            }
        }

        return nodeCount > 0 ? totalCoefficient / nodeCount : 0;
    }

    private double CalculateCentrality(DependencyGraph graph)
    {
        if (graph.Nodes.Count <= 1) return 0;

        var centrality = 0.0;
        var nodeCount = 0;

        foreach (var node in graph.Nodes)
        {
            var shortestPaths = 0;
            var pathsThroughNode = 0;

            foreach (var source in graph.Nodes.Where(n => n.Id != node.Id))
            {
                foreach (var target in graph.Nodes.Where(n => n.Id != node.Id && n.Id != source.Id))
                {
                    var path = graph.GetPath(source.Id, target.Id);
                    if (path != null)
                    {
                        shortestPaths++;
                        if (path.Any(n => n.Id == node.Id))
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
            totalProbability += await _errorClassifier.CalculateErrorProbabilityAsync(node);
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
                new Error(
                    type: node.NodeType,
                    message: "Node error analysis",
                    source: node.Id,
                    stackTrace: string.Empty
                ),
                environment: null,
                timestamp: DateTime.UtcNow
            );

            // Set additional properties
            errorContext.ComponentId = node.Id;
            errorContext.ErrorType = node.NodeType;
            errorContext.ServiceName = graph.Metadata.TryGetValue("ServiceName", out var svc) ? svc?.ToString() : string.Empty;
            
            // Add metadata using the proper method
            errorContext.AddMetadata("NodeId", node.Id);
            errorContext.AddMetadata("NodeType", node.NodeType);
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

        var errorNodes = graph.Nodes.Where(n => n.ErrorProbability > 0.5).ToList();
        if (!errorNodes.Any())
            return 0;

        var totalSpread = 0.0;
        foreach (var errorNode in errorNodes)
        {
            var errorContext = new ErrorContext(
                new Error(
                    type: errorNode.NodeType,
                    message: "Error spread analysis",
                    source: errorNode.Id,
                    stackTrace: string.Empty
                ),
                environment: null,
                timestamp: DateTime.UtcNow
            );

            errorContext.ComponentId = errorNode.Id;
            errorContext.AddMetadata("NodeId", errorNode.Id);
            errorContext.AddMetadata("NodeType", errorNode.NodeType);
            errorContext.AddMetadata("ErrorProbability", errorNode.ErrorProbability);

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
            // Health = 1 - ErrorProbability (clamped 0..1)
            var health = 1.0 - node.ErrorProbability;
            if (health < 0.0) health = 0.0;
            if (health > 1.0) health = 1.0;
            totalHealth += health;
        }

        return totalHealth / graph.Nodes.Count;
    }

    private async Task<double> CalculateSystemReliabilityAsync(DependencyGraph graph)
    {
        if (graph.Nodes.Count == 0)
            return 0;

        var totalReliability = 0.0;
        foreach (var node in graph.Nodes)
        {
            // Reliability = node.Reliability (clamped 0..1)
            var reliability = node.Reliability;
            if (reliability < 0.0) reliability = 0.0;
            if (reliability > 1.0) reliability = 1.0;
            totalReliability += reliability;
        }

        return totalReliability / graph.Nodes.Count;
    }
} 