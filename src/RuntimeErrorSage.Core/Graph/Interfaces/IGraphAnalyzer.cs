using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Graph.Models;

namespace RuntimeErrorSage.Core.Graph.Interfaces
{
    /// <summary>
    /// Defines the interface for graph-based context analysis.
    /// </summary>
    public interface IGraphAnalyzer
    {
        /// <summary>
        /// Analyzes the error context using graph-based analysis.
        /// </summary>
        /// <param name="context">The error context to analyze</param>
        /// <returns>The graph analysis result</returns>
        Task<GraphAnalysisResult> AnalyzeContextAsync(ErrorContext context);

        /// <summary>
        /// Builds a dependency graph from the error context.
        /// </summary>
        /// <param name="context">The error context</param>
        /// <returns>The dependency graph</returns>
        Task<DependencyGraph> BuildDependencyGraphAsync(ErrorContext context);

        /// <summary>
        /// Analyzes the impact of an error on the system.
        /// </summary>
        /// <param name="context">The error context</param>
        /// <param name="graph">The dependency graph</param>
        /// <returns>The impact analysis result</returns>
        Task<ImpactAnalysisResult> AnalyzeImpactAsync(ErrorContext context, DependencyGraph graph);

        /// <summary>
        /// Finds related errors in the system.
        /// </summary>
        /// <param name="context">The error context</param>
        /// <param name="graph">The dependency graph</param>
        /// <returns>A collection of related errors</returns>
        Task<IEnumerable<RelatedError>> FindRelatedErrorsAsync(ErrorContext context, DependencyGraph graph);

        /// <summary>
        /// Validates the graph analysis configuration.
        /// </summary>
        /// <returns>True if the configuration is valid, false otherwise</returns>
        Task<bool> ValidateConfigurationAsync();
    }
} 