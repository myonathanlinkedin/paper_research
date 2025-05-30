using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;

namespace RuntimeErrorSage.Application.Remediation.Interfaces
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
