using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Graph;

namespace RuntimeErrorSage.Application.Graph.Interfaces
{
    /// <summary>
    /// Interface for analyzing dependency graphs.
    /// </summary>
    public interface IDependencyGraphAnalyzer
    {
        /// <summary>
        /// Analyzes the dependency graph for impact.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        /// <param name="startNodeId">The ID of the node to start the analysis from.</param>
        /// <returns>The impact analysis result.</returns>
        Task<ImpactAnalysisResult> AnalyzeImpactAsync(DependencyGraph graph, string startNodeId);

        /// <summary>
        /// Analyzes the dependency graph for critical paths.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        /// <returns>The list of critical paths.</returns>
        Task<Collection<GraphPath>> AnalyzeCriticalPathsAsync(DependencyGraph graph);

        /// <summary>
        /// Identifies nodes with high error probability.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        /// <param name="threshold">The probability threshold.</param>
        /// <returns>The list of high-risk nodes.</returns>
        Task<Collection<DependencyNode>> IdentifyHighRiskNodesAsync(DependencyGraph graph, double threshold = 0.7);

        /// <summary>
        /// Calculates error propagation paths in the graph.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        /// <param name="errorNodeId">The ID of the error node.</param>
        /// <returns>The list of error propagation paths.</returns>
        Task<Collection<GraphPath>> CalculateErrorPropagationPathsAsync(DependencyGraph graph, string errorNodeId);

        /// <summary>
        /// Analyzes the root cause of an error in the graph.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        /// <param name="errorNodeId">The ID of the error node.</param>
        /// <returns>The root cause analysis result.</returns>
        Task<RootCauseAnalysisResult> AnalyzeRootCauseAsync(DependencyGraph graph, string errorNodeId);

        /// <summary>
        /// Finds all dependency cycles in the graph.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        /// <returns>The list of cycles found in the graph.</returns>
        Task<Collection<GraphCycle>> FindCyclesAsync(DependencyGraph graph);

        /// <summary>
        /// Calculates the centrality of nodes in the graph.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        /// <returns>A dictionary mapping node IDs to their centrality scores.</returns>
        Task<Dictionary<string, double>> CalculateCentralityAsync(DependencyGraph graph);
    }
} 






