using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Services;

/// <summary>
/// Provides functionality for graph operations related to error relationships.
/// </summary>
public class ErrorRelationshipGraphOperations
{
    /// <summary>
    /// Finds errors in the same component.
    /// </summary>
    public IEnumerable<RelatedError> FindErrorsInComponent(ErrorContext context, DependencyGraph graph)
    {
        if (string.IsNullOrEmpty(context.ComponentId))
            return Enumerable.Empty<RelatedError>();

        return graph.Nodes
            .Where(n => n.Value.Type == GraphNodeType.Error.ToString() && 
                       n.Value.Metadata.TryGetValue("ComponentId", out var compId) && 
                       compId?.ToString() == context.ComponentId)
            .Select(n => new RelatedError
            {
                ErrorId = n.Value.Id,
                RelationshipType = ErrorRelationshipType.RelatedTo,
                ConfidenceLevel = 0.8,
                Timestamp = DateTime.UtcNow
            });
    }

    /// <summary>
    /// Finds errors in dependent components.
    /// </summary>
    public IEnumerable<RelatedError> FindErrorsInDependentComponents(ErrorContext context, DependencyGraph graph)
    {
        if (string.IsNullOrEmpty(context.ComponentId))
            return Enumerable.Empty<RelatedError>();

        var dependentComponents = graph.Edges
            .Where(e => e.Target.Id == context.ComponentId)
            .Select(e => e.Source.Id)
            .Distinct();

        return dependentComponents
            .SelectMany(componentId => graph.Nodes
                .Where(n => n.Value.Type == GraphNodeType.Error.ToString() && 
                           n.Value.Metadata.TryGetValue("ComponentId", out var compId) && 
                           compId?.ToString() == componentId)
                .Select(n => new RelatedError
                {
                    ErrorId = n.Value.Id,
                    RelationshipType = ErrorRelationshipType.DependsOn,
                    ConfidenceLevel = 0.6,
                    Timestamp = DateTime.UtcNow
                }));
    }

    /// <summary>
    /// Finds errors with similar patterns.
    /// </summary>
    public IEnumerable<RelatedError> FindErrorsWithSimilarPatterns(
        ErrorContext context, 
        DependencyGraph graph,
        ErrorPatternMatcher patternMatcher)
    {
        return graph.Nodes
            .Where(n => n.Value.Type == GraphNodeType.Error.ToString())
            .Where(n => n.Value.Metadata.TryGetValue("Message", out var errorMessage) && errorMessage != null)
            .Where(n =>
            {
                var error = new RuntimeError(
                    message: n.Value.Metadata["Message"].ToString(),
                    source: n.Value.Metadata.TryGetValue("ComponentId", out var compId) ? compId?.ToString() : "Unknown",
                    type: "Unknown",
                    stackTrace: string.Empty
                );

                var nodeError = new ErrorContext(
                    error: error,
                    context: n.Value.Metadata.TryGetValue("ComponentId", out var componentId) ? componentId?.ToString() : "Unknown",
                    timestamp: DateTime.UtcNow
                );

                return patternMatcher.HaveSimilarPatterns(context, nodeError);
            })
            .Select(n => new RelatedError
            {
                ErrorId = n.Value.Id,
                RelationshipType = ErrorRelationshipType.SimilarTo,
                ConfidenceLevel = 0.4,
                Timestamp = DateTime.UtcNow
            });
    }
} 
