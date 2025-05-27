using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Graph.Interfaces;

/// <summary>
/// Interface for analyzing graphs and their dependencies.
/// </summary>
public interface IGraphAnalyzer
{
    /// <summary>
    /// Analyzes a dependency graph.
    /// </summary>
    /// <param name="graph">The dependency graph to analyze.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the analysis result.</returns>
    Task<GraphAnalysisResult> AnalyzeGraphAsync(DependencyGraph graph);

    /// <summary>
    /// Analyzes the impact of a node on the graph.
    /// </summary>
    /// <param name="graph">The dependency graph.</param>
    /// <param name="nodeId">The node ID to analyze.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the impact analysis result.</returns>
    Task<ImpactAnalysisResult> AnalyzeNodeImpactAsync(DependencyGraph graph, string nodeId);

    /// <summary>
    /// Calculates the shortest path between two nodes in the graph.
    /// </summary>
    /// <param name="graph">The dependency graph.</param>
    /// <param name="sourceId">The source node ID.</param>
    /// <param name="targetId">The target node ID.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the shortest path as a list of DependencyNode.</returns>
    Task<List<DependencyNode>> CalculateShortestPathAsync(DependencyGraph graph, string sourceId, string targetId);

    /// <summary>
    /// Updates the graph metrics.
    /// </summary>
    /// <param name="graph">The dependency graph to update metrics for.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateGraphMetricsAsync(DependencyGraph graph);
} 