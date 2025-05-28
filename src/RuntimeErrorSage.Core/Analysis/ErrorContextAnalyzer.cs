using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Analysis.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Services.Interfaces;

namespace RuntimeErrorSage.Core.Analysis;

/// <summary>
/// Analyzes error contexts and builds dependency graphs.
/// </summary>
public class ErrorContextAnalyzer : IErrorContextAnalyzer
{
    private readonly ILogger<ErrorContextAnalyzer> _logger;
    private readonly IGraphBuilder _graphBuilder;
    private readonly IImpactAnalyzer _impactAnalyzer;
    private readonly IErrorRelationshipAnalyzer _relationshipAnalyzer;

    public ErrorContextAnalyzer(
        ILogger<ErrorContextAnalyzer> logger,
        IGraphBuilder graphBuilder,
        IImpactAnalyzer impactAnalyzer,
        IErrorRelationshipAnalyzer relationshipAnalyzer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
        _impactAnalyzer = impactAnalyzer ?? throw new ArgumentNullException(nameof(impactAnalyzer));
        _relationshipAnalyzer = relationshipAnalyzer ?? throw new ArgumentNullException(nameof(relationshipAnalyzer));
    }

    /// <inheritdoc />
    public async Task<GraphAnalysisResult> AnalyzeErrorContextAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Starting error context analysis for error {ErrorId}", context.ErrorId);

            var result = new GraphAnalysisResult
            {
                StartTime = DateTime.UtcNow,
                CorrelationId = context.CorrelationId,
                Timestamp = DateTime.UtcNow
            };

            // Build dependency graph
            result.DependencyGraph = await _graphBuilder.BuildGraphAsync(context);

            // Analyze impact
            var impactResults = await _impactAnalyzer.AnalyzeImpactAsync(context, result.DependencyGraph);
            result.ImpactResults = impactResults;

            // Find related errors
            result.RelatedErrors = await _relationshipAnalyzer.FindRelatedErrorsAsync(context, result.DependencyGraph);

            result.EndTime = DateTime.UtcNow;
            result.Status = AnalysisStatus.Completed;

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing error context for error {ErrorId}", context.ErrorId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ErrorAnalysisResult> AnalyzeContextAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Starting context analysis for error {ErrorId}", context.ErrorId);

            var result = new ErrorAnalysisResult
            {
                ErrorId = context.ErrorId,
                CorrelationId = context.CorrelationId,
                Timestamp = DateTime.UtcNow,
                Status = AnalysisStatus.InProgress
            };

            // Analyze error context
            var graphAnalysis = await AnalyzeErrorContextAsync(context);
            if (!graphAnalysis.IsValid)
            {
                result.Status = AnalysisStatus.Failed;
                result.Details["Error"] = graphAnalysis.ErrorMessage;
                return result;
            }

            // Update result with graph analysis data
            result.DependencyGraph = graphAnalysis.DependencyGraph;
            result.ImpactResults = graphAnalysis.ImpactResults;
            result.RelatedErrors = graphAnalysis.RelatedErrors;
            result.Metrics = graphAnalysis.Metrics;

            result.Status = AnalysisStatus.Completed;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing context for error {ErrorId}", context.ErrorId);
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
    public async Task<ImpactAnalysisResult> AnalyzeImpactAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Analyzing impact for error {ErrorId}", context.ErrorId);

            // Build dependency graph if not already present
            var graph = context.DependencyGraph ?? await BuildDependencyGraphAsync(context);

            // Analyze impact
            var impactResults = await _impactAnalyzer.AnalyzeImpactAsync(context, graph);
            return impactResults.FirstOrDefault() ?? new ImpactAnalysisResult
            {
                IsValid = false,
                ErrorMessage = "No impact analysis results available",
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing impact for error {ErrorId}", context.ErrorId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<List<DependencyNode>> CalculateShortestPathAsync(string sourceId, string targetId)
    {
        // TODO: Implement actual shortest path logic
        await Task.CompletedTask;
        return new List<DependencyNode>();
    }

    /// <inheritdoc />
    public async Task UpdateGraphMetricsAsync(ErrorContext context)
    {
        // TODO: Implement actual graph metrics update logic
        await Task.CompletedTask;
    }
} 