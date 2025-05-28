using System;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Options;
using RuntimeErrorSage.Core.Remediation.Interfaces;
using RemediationResult = RuntimeErrorSage.Core.Models.Remediation.RemediationResult;
using RuntimeErrorSage.Core.Analysis;
using RuntimeErrorSage.Core.Remediation;
using RuntimeErrorSage.Core.MCP;
using RuntimeErrorSage.Core.LLM;
using RuntimeErrorSage.Core.Validation;
using RuntimeErrorSage.Core.Graph;
using System.ComponentModel.DataAnnotations;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Models.LLM;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Runtime.Interfaces
{
    /// <summary>
    /// Defines the core service interface for RuntimeErrorSage runtime intelligence.
    /// </summary>
    public interface IRuntimeErrorSageService
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
        void Configure(RuntimeErrorSageOptions options);

        /// <summary>
        ///   Analyzes an error context to generate an error analysis result.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<ErrorAnalysisResult> AnalyzeErrorAsync(ErrorContext context);

        /// <summary>
        /// Remediates an error context based on the analysis result.   
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<RemediationResult> RemediateErrorAsync(ErrorContext context);

        /// <summary>
        ///     Validates the error context using registered validators.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<ValidationResult> ValidateContextAsync(ErrorContext context);

        /// <summary>
        /// Analyzes the context graph to generate a graph analysis result. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<GraphAnalysisResult> AnalyzeContextGraphAsync(ErrorContext context);

        /// <summary>
        ///     
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<LLMAnalysisResult> AnalyzeWithLLMAsync(ErrorContext context);
    }
} 
