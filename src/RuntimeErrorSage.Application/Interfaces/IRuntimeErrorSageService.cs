using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Options;
using RemediationResult = RuntimeErrorSage.Application.Models.Remediation.RemediationResult;
using RuntimeErrorSage.Application.Analysis;
using RuntimeErrorSage.Application.Remediation;
using RuntimeErrorSage.Application.MCP;
using RuntimeErrorSage.Application.LLM;
using RuntimeErrorSage.Application.Validation;
using RuntimeErrorSage.Application.Graph;
using System.ComponentModel.DataAnnotations;
using RuntimeErrorSage.Application.Models.Graph;
using RuntimeErrorSage.Application.Models.LLM;

namespace RuntimeErrorSage.Application.Runtime.Interfaces
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
        void RegisterRemediationStrategy(RuntimeErrorSage.Application.Models.Remediation.Interfaces.IRemediationStrategy strategy);

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







