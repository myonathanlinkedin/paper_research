using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.Graph;

namespace RuntimeErrorSage.Model.Analysis.Interfaces
{
    /// <summary>
    /// Interface for analyzing dependency graphs in the context of error analysis.
    /// </summary>
    public interface IDependencyGraphAnalyzer
    {
        /// <summary>
        /// Analyzes the dependency graph for a given error context.
        /// </summary>
        /// <param name="dependencyGraph">The dependency graph to analyze.</param>
        /// <returns>A dictionary of component IDs to their analysis results.</returns>
        Dictionary<string, object> AnalyzeDependencies(DependencyGraph dependencyGraph);

        /// <summary>
        /// Identifies affected components in the dependency graph.
        /// </summary>
        /// <param name="dependencyGraph">The dependency graph to analyze.</param>
        /// <param name="sourceComponentId">The ID of the source component.</param>
        /// <returns>A list of affected graph nodes.</returns>
        List<GraphNode> IdentifyAffectedComponents(DependencyGraph dependencyGraph, string sourceComponentId);

        /// <summary>
        /// Calculates the impact score for a component in the dependency graph.
        /// </summary>
        /// <param name="dependencyGraph">The dependency graph to analyze.</param>
        /// <param name="componentId">The ID of the component to analyze.</param>
        /// <returns>A score between 0 and 1 indicating the impact level.</returns>
        double CalculateImpactScore(DependencyGraph dependencyGraph, string componentId);

        /// <summary>
        /// Analyzes the context and returns graph analysis results
        /// </summary>
        /// <param name="context">The error context to analyze</param>
        /// <returns>The graph analysis result</returns>
        Task<GraphAnalysisResult> AnalyzeContextAsync(ErrorContext context);

        /// <summary>
        /// Validates the analyzer's configuration
        /// </summary>
        /// <returns>True if the configuration is valid; otherwise, false</returns>
        Task<bool> ValidateConfigurationAsync();
    }
} 
