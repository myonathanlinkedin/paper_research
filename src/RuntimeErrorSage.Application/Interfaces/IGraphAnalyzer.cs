using System.Collections.ObjectModel;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Graph;

namespace RuntimeErrorSage.Application.Remediation.Interfaces
{
    /// <summary>
    /// Interface for analyzing error context graphs for remediation purposes.
    /// </summary>
    public interface IGraphAnalyzer
    {
        /// <summary>
        /// Analyzes the error context graph to determine component health, relationships, and error propagation.
        /// </summary>
        /// <param name="context">The error context containing component graph data.</param>
        /// <returns>A graph analysis result containing component health, relationships, and error propagation data.</returns>
        Task<GraphAnalysis> AnalyzeContextGraphAsync(ErrorContext context);
    }
} 






