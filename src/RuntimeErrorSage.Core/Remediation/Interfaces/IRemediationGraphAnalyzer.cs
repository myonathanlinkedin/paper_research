using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    /// <summary>
    /// Interface for analyzing error context graphs for remediation purposes.
    /// </summary>
    public interface IRemediationGraphAnalyzer
    {
        /// <summary>
        /// Analyzes a context graph to determine component health and relationships.
        /// </summary>
        /// <param name="context">The error context to analyze.</param>
        /// <returns>The graph analysis result.</returns>
        Task<GraphAnalysis> AnalyzeContextGraphAsync(ErrorContext context);
    }
} 
