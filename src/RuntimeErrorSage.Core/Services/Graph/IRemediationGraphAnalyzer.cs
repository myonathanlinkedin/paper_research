using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Services.Graph
{
    /// <summary>
    /// Interface for analyzing graphs for remediation purposes.
    /// </summary>
    public interface IRemediationGraphAnalyzer
    {
        /// <summary>
        /// Analyzes the graph for remediation.
        /// </summary>
        /// <param name="errorId">The error identifier.</param>
        /// <returns>The graph analysis.</returns>
        Task<GraphAnalysis> AnalyzeGraphAsync(string errorId);
    }
} 