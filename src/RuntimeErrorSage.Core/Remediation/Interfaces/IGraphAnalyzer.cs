using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Remediation.Models.Analysis;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    /// <summary>
    /// Interface for analyzing error context graphs for remediation purposes.
    /// </summary>
    public interface IRemediationGraphAnalyzer
    {
        /// <summary>
        /// Analyzes the error context graph to determine component health, relationships, and error propagation.
        /// </summary>
        /// <param name="context">The error context containing component graph data.</param>
        /// <returns>A graph analysis result containing component health, relationships, and error propagation data.</returns>
        Task<GraphAnalysis> AnalyzeContextGraphAsync(ErrorContext context);
    }
} 