using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Options;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RuntimeErrorSage.Core.Interfaces.MCP;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Defines the interface for error analysis.
    /// </summary>
    public interface IErrorAnalyzer
    {
        /// <summary>
        /// Analyzes an error and provides insights and suggestions.
        /// </summary>
        /// <param name="exception">The exception that occurred</param>
        /// <param name="context">Additional context information</param>
        /// <returns>An analysis result</returns>
        Task<ErrorAnalysisResult> AnalyzeErrorAsync(Exception exception, ErrorContext context);

        /// <summary>
        /// Gets the status of a previously requested analysis.
        /// </summary>
        /// <param name="correlationId">The correlation ID of the analysis</param>
        /// <returns>The analysis result if available, otherwise null</returns>
        Task<ErrorAnalysisResult?> GetAnalysisStatusAsync(string correlationId);
    }
} 
