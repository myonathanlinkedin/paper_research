using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Interfaces;

/// <summary>
/// Interface for analyzing error contexts and building dependency graphs.
/// </summary>
public interface IErrorContextAnalyzer
{
    /// <summary>
    /// Analyzes an error context and builds a dependency graph.
    /// </summary>
    /// <param name="context">The error context to analyze.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the analysis result.</returns>
    Task<GraphAnalysisResult> AnalyzeErrorContextAsync(ErrorContext context);

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
    Task<ImpactAnalysisResult> AnalyzeImpactAsync(ErrorContext context);

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