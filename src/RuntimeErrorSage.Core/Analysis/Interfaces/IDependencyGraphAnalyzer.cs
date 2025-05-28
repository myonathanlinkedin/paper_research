using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Dependency;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Analysis.Interfaces
{
    /// <summary>
    /// Interface for analyzing dependency graphs.
    /// </summary>
    public interface IDependencyGraphAnalyzer
    {
        /// <summary>
        /// Builds a dependency graph for a given error context.
        /// </summary>
        /// <param name="errorContext">The error context.</param>
        /// <returns>The dependency graph.</returns>
        Task<DependencyGraph> BuildDependencyGraphAsync(ErrorContext errorContext);

        /// <summary>
        /// Analyzes a dependency graph to identify potential error sources.
        /// </summary>
        /// <param name="graph">The dependency graph to analyze.</param>
        /// <param name="errorContext">The error context.</param>
        /// <returns>A list of potential error sources.</returns>
        Task<List<PotentialErrorSource>> AnalyzeGraphAsync(DependencyGraph graph, ErrorContext errorContext);

        /// <summary>
        /// Analyzes error propagation through a dependency graph.
        /// </summary>
        /// <param name="graph">The dependency graph to analyze.</param>
        /// <param name="errorNode">The node where the error originated.</param>
        /// <returns>A list of impacted nodes.</returns>
        Task<List<GraphNode>> AnalyzeErrorPropagationAsync(DependencyGraph graph, GraphNode errorNode);

        /// <summary>
        /// Identifies critical paths in a dependency graph.
        /// </summary>
        /// <param name="graph">The dependency graph to analyze.</param>
        /// <returns>A list of critical paths.</returns>
        Task<List<GraphPath>> IdentifyCriticalPathsAsync(DependencyGraph graph);

        /// <summary>
        /// Calculates the impact score for a node in the dependency graph.
        /// </summary>
        /// <param name="graph">The dependency graph.</param>
        /// <param name="node">The node to calculate the impact score for.</param>
        /// <returns>The impact score.</returns>
        double CalculateImpactScore(DependencyGraph graph, GraphNode node);
    }
} 