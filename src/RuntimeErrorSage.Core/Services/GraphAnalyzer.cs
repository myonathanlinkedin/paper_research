using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Services.Interfaces;
using RelatedErrorModel = RuntimeErrorSage.Core.Models.Error.RelatedError;

namespace RuntimeErrorSage.Core.Services;

/// <summary>
/// Service for analyzing error contexts using graph-based analysis.
/// </summary>
public class GraphAnalyzer : IGraphAnalyzer
{
    private readonly ILogger<GraphAnalyzer> _logger;
    private readonly IGraphBuilder _graphBuilder;
    private readonly IImpactAnalyzer _impactAnalyzer;
    private readonly IErrorRelationshipAnalyzer _relationshipAnalyzer;

    public GraphAnalyzer(
        ILogger<GraphAnalyzer> logger,
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
    public async Task<GraphAnalysisResult> AnalyzeContextAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Starting graph analysis for error {ErrorId}", context.ErrorId);

            var result = new GraphAnalysisResult
            {
                StartTime = DateTime.UtcNow
            };

            // Build dependency graph
            result.DependencyGraph = await _graphBuilder.BuildGraphAsync(context);

            // Analyze impact
            result.ImpactResults = await _impactAnalyzer.AnalyzeImpactAsync(context, result.DependencyGraph);

            // Find related errors
            result.RelatedErrors = await _relationshipAnalyzer.FindRelatedErrorsAsync(context, result.DependencyGraph);

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
            return await _impactAnalyzer.AnalyzeImpactAsync(context, graph);
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
            
            var graphBuilderValid = await _graphBuilder.ValidateConfigurationAsync();
            var impactAnalyzerValid = await _impactAnalyzer.ValidateConfigurationAsync();
            var relationshipAnalyzerValid = await _relationshipAnalyzer.ValidateConfigurationAsync();

            return graphBuilderValid && impactAnalyzerValid && relationshipAnalyzerValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating graph analyzer configuration");
            throw;
        }
    }
} 