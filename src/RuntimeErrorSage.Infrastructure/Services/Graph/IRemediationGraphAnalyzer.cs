using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Graph;

namespace RuntimeErrorSage.Application.Services.Graph
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
