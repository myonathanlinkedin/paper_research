using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CodeSage.Core.Interfaces;
using CodeSage.Core.Models;
using CodeSage.Core.Options;
using CodeSage.Core.Remediation.Interfaces;
using CodeSage.Core.Interfaces.MCP;
using CodeSage.Core.Models.Error;

namespace CodeSage.Core.Interfaces
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