using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using RelatedErrorModel = RuntimeErrorSage.Domain.Models.Error.RelatedError;

namespace RuntimeErrorSage.Application.Services;

/// <summary>
/// Service for analyzing relationships between errors.
/// </summary>
public class ErrorRelationshipAnalyzer : IErrorRelationshipAnalyzer
{
    private readonly ILogger<ErrorRelationshipAnalyzer> _logger;
    private readonly ErrorRelationshipAnalysis _analysis;

    public ErrorRelationshipAnalyzer(
        ILogger<ErrorRelationshipAnalyzer> logger,
        ErrorRelationshipAnalysis analysis)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(analysis);

        _logger = logger;
        _analysis = analysis;
    }

    /// <inheritdoc />
    public async Task<ErrorRelationship> AnalyzeRelationshipAsync(ErrorContext error1, ErrorContext error2)
    {
        ArgumentNullException.ThrowIfNull(error1);
        ArgumentNullException.ThrowIfNull(error2);

        try
        {
            _logger.LogInformation("Analyzing relationship between errors {Error1} and {Error2}", error1.ErrorId, error2.ErrorId);

            var relationship = new ErrorRelationship
            {
                SourceErrorId = error1.ErrorId,
                TargetErrorId = error2.ErrorId,
                Timestamp = DateTime.UtcNow
            };

            // Check if errors are in the same component
            if (error1.ComponentId == error2.ComponentId)
            {
                relationship.RelationshipType = ErrorRelationshipType.RelatedTo;
                relationship.Confidence = 0.8;
            }
            // Check if errors are in dependent components
            else if (_analysis.AreComponentsDependent(error1.ComponentId, error2.ComponentId))
            {
                relationship.RelationshipType = ErrorRelationshipType.DependsOn;
                relationship.Confidence = 0.6;
            }
            // Check if errors have similar patterns
            else if (_analysis.HaveSimilarPatterns(error1, error2))
            {
                relationship.RelationshipType = ErrorRelationshipType.SimilarTo;
                relationship.Confidence = 0.4;
            }
            else
            {
                relationship.RelationshipType = ErrorRelationshipType.Unknown;
                relationship.Confidence = 0.1;
            }

            await Task.CompletedTask;
            return relationship;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing relationship between errors {Error1} and {Error2}", error1.ErrorId, error2.ErrorId);
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
            _logger.LogError(ex, "Error validating error relationship analyzer configuration");
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RelatedErrorModel>> AnalyzeRelationshipsAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Analyzing relationships for error {ErrorId}", context.ErrorId);

            var relationships = new List<RelatedErrorModel>();

            // Get previous errors from context
            if (context.PreviousErrors != null)
            {
                foreach (var previousError in context.PreviousErrors)
                {
                    var relationship = await AnalyzeRelationshipAsync(context, previousError);
                    relationships.Add(new RelatedErrorModel
                    {
                        ErrorId = previousError.ErrorId,
                        RelationshipType = relationship.RelationshipType,
                        ConfidenceLevel = relationship.Confidence,
                        Timestamp = DateTime.UtcNow
                    });
                }
            }

            return relationships.OrderByDescending(r => r.ConfidenceLevel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing relationships for error {ErrorId}", context.ErrorId);
            throw;
        }
    }

    /// <inheritdoc />
    public ErrorRelationship GetRelationshipType(RuntimeError source, RuntimeError target)
    {
        try
        {
            _logger.LogInformation("Getting relationship type between errors {Source} and {Target}", source.Id, target.Id);

            // Check if errors are in the same component
            if (source.ComponentId == target.ComponentId)
            {
                return new ErrorRelationship
                {
                    SourceErrorId = source.Id,
                    TargetErrorId = target.Id,
                    RelationshipType = ErrorRelationshipType.Sibling,
                    Confidence = 0.8,
                    Description = "These errors appear to be related based on error type",
                    Timestamp = DateTime.UtcNow,
                    IsBidirectional = false
                };
            }

            // Check if errors are in dependent components
            if (_analysis.AreComponentsDependent(source.ComponentId, target.ComponentId))
            {
                return new ErrorRelationship
                {
                    SourceErrorId = source.Id,
                    TargetErrorId = target.Id,
                    RelationshipType = ErrorRelationshipType.Dependency,
                    Confidence = 0.6,
                    Description = "These errors appear to be related based on error type",
                    Timestamp = DateTime.UtcNow,
                    IsBidirectional = false
                };
            }

            // Check if errors have similar patterns
            if (_analysis.HaveSimilarPatterns(source, target))
            {
                return new ErrorRelationship
                {
                    SourceErrorId = source.Id,
                    TargetErrorId = target.Id,
                    RelationshipType = ErrorRelationshipType.Correlation,
                    Confidence = 0.4,
                    Description = "These errors appear to be related based on error type",
                    Timestamp = DateTime.UtcNow,
                    IsBidirectional = false
                };
            }

            // Check if errors have a temporal relationship
            if (_analysis.HasTemporalRelationship(source, target))
            {
                return new ErrorRelationship
                {
                    SourceErrorId = source.Id,
                    TargetErrorId = target.Id,
                    RelationshipType = ErrorRelationshipType.Temporal,
                    Confidence = 0.5,
                    Description = "These errors appear to be related based on timing",
                    Timestamp = DateTime.UtcNow,
                    IsBidirectional = false
                };
            }

            // Check if errors have a spatial relationship
            if (_analysis.HasSpatialRelationship(source, target))
            {
                return new ErrorRelationship
                {
                    SourceErrorId = source.Id,
                    TargetErrorId = target.Id,
                    RelationshipType = ErrorRelationshipType.Spatial,
                    Confidence = 0.3,
                    Description = "These errors appear to be related based on location",
                    Timestamp = DateTime.UtcNow,
                    IsBidirectional = false
                };
            }

            // Check if errors have a logical relationship
            if (_analysis.HasLogicalRelationship(source, target))
            {
                return new ErrorRelationship
                {
                    SourceErrorId = source.Id,
                    TargetErrorId = target.Id,
                    RelationshipType = ErrorRelationshipType.Logical,
                    Confidence = 0.2,
                    Description = "These errors appear to be related based on logic",
                    Timestamp = DateTime.UtcNow,
                    IsBidirectional = false
                };
            }

            return new ErrorRelationship
            {
                SourceErrorId = source.Id,
                TargetErrorId = target.Id,
                RelationshipType = ErrorRelationshipType.None,
                Confidence = 0.1,
                Description = "These errors do not appear to be related",
                Timestamp = DateTime.UtcNow,
                IsBidirectional = false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting relationship type between errors {Source} and {Target}", source.Id, target.Id);
            throw;
        }
    }

    private async Task<IEnumerable<RelatedErrorModel>> FindErrorsInComponentAsync(ErrorContext context, DependencyGraph graph)
    {
        var errors = new List<RelatedErrorModel>();

        if (string.IsNullOrEmpty(context.ComponentId))
            return errors;

        // Find all error nodes in the same component
        var componentErrors = graph.Nodes.ToList()
            .Where(n => n.Type == GraphNodeType.Error.ToString() && n.Metadata.TryGetValue("ComponentId", out var compId) && compId?.ToString() == context.ComponentId)
            .Select(n => new RelatedErrorModel
            {
                ErrorId = n.Id,
                RelationshipType = ErrorRelationshipType.RelatedTo,
                ConfidenceLevel = 0.8,
                Timestamp = DateTime.UtcNow
            });

        errors.AddRange(componentErrors);
        await Task.CompletedTask;
        return errors;
    }

    private async Task<IEnumerable<RelatedErrorModel>> FindErrorsInDependentComponentsAsync(ErrorContext context, DependencyGraph graph)
    {
        var errors = new List<RelatedErrorModel>();

        if (string.IsNullOrEmpty(context.ComponentId))
            return errors;

        // Find all components that depend on the current component
        var dependentComponents = graph.Edges
            .Where(e => e.Target.Id == context.ComponentId)
            .Select(e => e.Source.Id)
            .Distinct();

        // Find all errors in dependent components
        foreach (var componentId in dependentComponents)
        {
            var componentErrors = graph.Nodes.ToList()
                .Where(n => n.Type == GraphNodeType.Error.ToString() && n.Metadata.TryGetValue("ComponentId", out var compId) && compId?.ToString() == componentId)
                .Select(n => new RelatedErrorModel
                {
                    ErrorId = n.Id,
                    RelationshipType = ErrorRelationshipType.DependsOn,
                    ConfidenceLevel = 0.6,
                    Timestamp = DateTime.UtcNow
                });

            errors.AddRange(componentErrors);
        }

        await Task.CompletedTask;
        return errors;
    }

    private async Task<IEnumerable<RelatedErrorModel>> FindErrorsWithSimilarPatternsAsync(ErrorContext context, DependencyGraph graph)
    {
        var errors = new List<RelatedErrorModel>();

        // Find all error nodes
        var errorNodes = graph.Nodes.ToList()
            .Where(n => n.Type == GraphNodeType.Error.ToString())
            .ToList();

        // Compare each error with the context error
        foreach (var node in errorNodes)
        {
            if (node.Metadata.TryGetValue("ErrorMessage", out var errorMessage) && errorMessage != null)
            {
                // Create RuntimeError with correct constructor parameters
                var error = new RuntimeError(
                    type: "Unknown", // Use a default type since we don't have it
                    message: errorMessage.ToString(),
                    source: node.Metadata.TryGetValue("ComponentId", out var compId) ? compId?.ToString() : "Unknown",
                    stackTrace: string.Empty
                );

                // Create ErrorContext with correct constructor parameters
                var nodeError = new ErrorContext(
                    error: error,
                    context: node.Metadata.TryGetValue("ComponentId", out var componentId) ? componentId?.ToString() : "Unknown",
                    timestamp: DateTime.UtcNow
                );

                if (_analysis.HaveSimilarPatterns(context, nodeError))
                {
                    errors.Add(new RelatedErrorModel
                    {
                        ErrorId = node.Id,
                        RelationshipType = ErrorRelationshipType.SimilarTo,
                        ConfidenceLevel = 0.4,
                        Timestamp = DateTime.UtcNow
                    });
                }
            }
        }

        await Task.CompletedTask;
        return errors;
    }

    /// <summary>
    /// Finds related errors for a given error context and dependency graph.
    /// </summary>
    /// <param name="context">The error context.</param>
    /// <param name="graph">The dependency graph.</param>
    /// <returns>The list of related errors.</returns>
    public async Task<IEnumerable<RelatedErrorModel>> FindRelatedErrorsInGraphAsync(ErrorContext context, DependencyGraph graph)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(graph);

        try
        {
            _logger.LogInformation("Finding related errors for error {ErrorId}", context.ErrorId);

            var relatedErrors = new List<RelatedErrorModel>();

            // Find errors in the same component
            var componentErrors = await FindErrorsInComponentAsync(context, graph);
            relatedErrors.AddRange(componentErrors);

            // Find errors in dependent components
            var dependentErrors = await FindErrorsInDependentComponentsAsync(context, graph);
            relatedErrors.AddRange(dependentErrors);

            // Find errors with similar patterns
            var similarErrors = await FindErrorsWithSimilarPatternsAsync(context, graph);
            relatedErrors.AddRange(similarErrors);

            // Remove duplicates and sort by relationship strength
            return relatedErrors
                .Distinct(new RelatedErrorComparer())
                .OrderByDescending(e => e.ConfidenceLevel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding related errors for error {ErrorId}", context.ErrorId);
            throw;
        }
    }

    public async Task<List<RelatedErrorModel>> FindRelatedErrorsAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        try
        {
            _logger.LogInformation("Finding related errors for error {ErrorId} without graph data", context.ErrorId);
            
            // Use AnalyzeRelationshipsAsync as an alternative implementation without a graph
            var relationships = await AnalyzeRelationshipsAsync(context);
            return relationships.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding related errors for error {ErrorId}", context.ErrorId);
            return new List<RelatedErrorModel>();
        }
    }

    public bool IsEnabled => true;
    public string Name => "DefaultErrorRelationshipAnalyzer";
    public string Version => "1.0.0";

    public double AnalyzeRelationship(RuntimeError sourceError, RuntimeError targetError)
    {
        // Dummy implementation for interface compliance
        return 0.5;
    }

    public async Task<List<RelatedErrorModel>> GetRelatedErrorsAsync(RuntimeError error)
    {
        // Create ErrorContext with correct constructor parameters
        var context = new ErrorContext(
            error: error,
            context: "Error context",
            timestamp: DateTime.UtcNow
        );
        
        return await FindRelatedErrorsAsync(context);
    }

    public async Task<List<ErrorRelationship>> AnalyzeRelationshipsAsync(List<RuntimeError> errors)
    {
        // Dummy implementation for interface compliance
        return new List<ErrorRelationship>();
    }

    public async Task<double> CalculateRelationshipStrengthAsync(RuntimeError source, RuntimeError target)
    {
        // Dummy implementation for interface compliance
        return 0.5;
    }
} 
