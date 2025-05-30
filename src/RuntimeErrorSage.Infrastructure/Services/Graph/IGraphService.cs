using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Graph;

namespace RuntimeErrorSage.Application.Services.Graph
{
    /// <summary>
    /// Interface for graph-related operations.
    /// </summary>
    public interface IGraphService
    {
        /// <summary>
        /// Gets the graph for a specific error.
        /// </summary>
        /// <param name="errorId">The error identifier.</param>
        /// <returns>The graph analysis.</returns>
        Task<GraphAnalysis> GetGraphAsync(string errorId);

        /// <summary>
        /// Updates the graph with new information.
        /// </summary>
        /// <param name="graph">The graph to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateGraphAsync(GraphAnalysis graph);

        /// <summary>
        /// Validates the graph structure.
        /// </summary>
        /// <param name="graph">The graph to validate.</param>
        /// <returns>True if the graph is valid; otherwise, false.</returns>
        Task<bool> ValidateGraphAsync(GraphAnalysis graph);
    }
} 
