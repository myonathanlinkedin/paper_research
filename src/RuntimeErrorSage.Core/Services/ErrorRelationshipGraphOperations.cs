using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Services;

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

        return graph.Nodes.Values
            .Where(n => n.Type == GraphNodeType.Error && 
                       n.Metadata.TryGetValue("ComponentId", out var compId) && 
                       compId?.ToString() == context.ComponentId)
            .Select(n => new RelatedError
            {
                ErrorId = n.Id,
                RelationshipType = ErrorRelationshipType.SameComponent,
                RelationshipStrength = 0.8,
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
            .SelectMany(componentId => graph.Nodes.Values
                .Where(n => n.Type == GraphNodeType.Error && 
                           n.Metadata.TryGetValue("ComponentId", out var compId) && 
                           compId?.ToString() == componentId)
                .Select(n => new RelatedError
                {
                    ErrorId = n.Id,
                    RelationshipType = ErrorRelationshipType.Dependent,
                    RelationshipStrength = 0.6,
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
        return graph.Nodes.Values
            .Where(n => n.Type == GraphNodeType.Error)
            .Where(n => n.Metadata.TryGetValue("ErrorMessage", out var errorMessage) && errorMessage != null)
            .Where(n =>
            {
                var nodeError = new ErrorContext
                {
                    ErrorId = n.Id,
                    ErrorMessage = n.Metadata["ErrorMessage"].ToString(),
                    ComponentId = n.Metadata.TryGetValue("ComponentId", out var compId) ? compId?.ToString() : null
                };
                return patternMatcher.HaveSimilarPatterns(context, nodeError);
            })
            .Select(n => new RelatedError
            {
                ErrorId = n.Id,
                RelationshipType = ErrorRelationshipType.SimilarPattern,
                RelationshipStrength = 0.4,
                Timestamp = DateTime.UtcNow
            });
    }
} 