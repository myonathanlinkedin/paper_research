using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Models.Analysis;

namespace RuntimeErrorSage.Core.Analysis.Interfaces;

/// <summary>
/// Interface for analyzing error contexts.
/// </summary>
public interface IErrorContextAnalyzer
{
    /// <summary>
    /// Gets whether the analyzer is enabled.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Gets the analyzer name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the analyzer version.
    /// </summary>
    string Version { get; }

    /// <summary>
    /// Analyzes an error context for remediation options.
    /// </summary>
    /// <param name="context">The error context to analyze.</param>
    /// <returns>The remediation analysis result.</returns>
    Task<RemediationAnalysis> AnalyzeContextAsync(ErrorContext context);

    /// <summary>
    /// Gets related errors for an error context.
    /// </summary>
    /// <param name="context">The error context.</param>
    /// <returns>The list of related errors.</returns>
    Task<List<RelatedError>> GetRelatedErrorsAsync(ErrorContext context);

    /// <summary>
    /// Gets a dependency graph for an error context.
    /// </summary>
    /// <param name="context">The error context.</param>
    /// <returns>The error dependency graph.</returns>
    Task<ErrorDependencyGraph> GetErrorDependencyGraphAsync(ErrorContext context);

    /// <summary>
    /// Gets the root cause for an error context.
    /// </summary>
    /// <param name="context">The error context.</param>
    /// <returns>The root cause analysis.</returns>
    Task<RootCauseAnalysis> GetRootCauseAsync(ErrorContext context);

    /// <summary>
    /// Analyzes an error context and builds a dependency graph.
    /// </summary>
    /// <param name="context">The error context to analyze.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the analysis result.</returns>
    Task<Models.Analysis.GraphAnalysisResult> AnalyzeErrorContextAsync(ErrorContext context);

    /// <summary>
    /// Builds a dependency graph for the given error context.
    /// </summary>
    /// <param name="context">The error context to build the graph for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the dependency graph.</returns>
    Task<DependencyGraph> BuildDependencyGraphAsync(ErrorContext context);

    /// <summary>
    /// Analyzes the impact of an error on the system.
    /// </summary>
    /// <param name="context">The error context to analyze.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the impact analysis result.</returns>
    Task<Models.Analysis.ImpactAnalysisResult> AnalyzeImpactAsync(ErrorContext context);

    /// <summary>
    /// Calculates the shortest path between two nodes in the dependency graph.
    /// </summary>
    /// <param name="sourceId">The source node ID.</param>
    /// <param name="targetId">The target node ID.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the shortest path as a list of DependencyNode.</returns>
    Task<List<DependencyNode>> CalculateShortestPathAsync(string sourceId, string targetId);

    /// <summary>
    /// Updates the graph metrics for the given error context.
    /// </summary>
    /// <param name="context">The error context to update metrics for.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateGraphMetricsAsync(ErrorContext context);
} 
