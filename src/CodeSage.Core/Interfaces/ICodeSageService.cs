using System;
using System.Threading.Tasks;
using CodeSage.Core.Models;
using CodeSage.Core.Options;
using CodeSage.Core.Remediation.Interfaces;
using CodeSage.Core.Models.Error;

namespace CodeSage.Core.Interfaces
{
    /// <summary>
    /// Defines the core service interface for CodeSage runtime intelligence.
    /// </summary>
    public interface ICodeSageService
    {
        /// <summary>
        /// Processes an exception and generates an analysis result.
        /// </summary>
        /// <param name="exception">The exception to analyze</param>
        /// <param name="context">Additional context information</param>
        /// <returns>The analysis result containing explanations and suggestions</returns>
        Task<ErrorAnalysisResult> ProcessExceptionAsync(Exception exception, ErrorContext context);

        /// <summary>
        /// Attempts to apply automated remediation based on the analysis result.
        /// </summary>
        /// <param name="analysisResult">The analysis result to act upon</param>
        /// <returns>The result of the remediation attempt</returns>
        Task<RemediationResult> ApplyRemediationAsync(ErrorAnalysisResult analysisResult);

        /// <summary>
        /// Enriches the error context with additional runtime information.
        /// </summary>
        /// <param name="context">The base error context</param>
        /// <returns>An enriched error context</returns>
        Task<ErrorContext> EnrichContextAsync(ErrorContext context);

        /// <summary>
        /// Registers a custom remediation strategy.
        /// </summary>
        /// <param name="strategy">The remediation strategy to register</param>
        void RegisterRemediationStrategy(IRemediationStrategy strategy);

        /// <summary>
        /// Configures the service with specific settings.
        /// </summary>
        /// <param name="options">The configuration options</param>
        void Configure(CodeSageOptions options);
    }
} 