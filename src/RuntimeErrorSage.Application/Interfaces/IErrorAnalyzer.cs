using System;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.LLM;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Graph;

namespace RuntimeErrorSage.Application.Analysis.Interfaces;

/// <summary>
/// Defines the interface for error analyzers.
/// </summary>
public interface IErrorAnalyzer
{
    /// <summary>
    /// Analyzes an error.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>The analysis.</returns>
    Task<ErrorAnalysis> AnalyzeAsync(RuntimeError error);

    /// <summary>
    /// Analyzes an error context.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>The analysis.</returns>
    Task<ErrorAnalysis> AnalyzeContextAsync(ErrorContext context);

    /// <summary>
    /// Validates an analysis.
    /// </summary>
    /// <param name="analysis">The analysis.</param>
    /// <returns>True if the analysis is valid; otherwise, false.</returns>
    bool ValidateAnalysis(ErrorAnalysis analysis);

    /// <summary>
    /// Analyzes an error and its context
    /// </summary>
    /// <param name="exception">The exception to analyze</param>
    /// <param name="context">The error context</param>
    /// <returns>The analysis result</returns>
    Task<ErrorAnalysisResult> AnalyzeErrorAsync(Exception exception, ErrorContext context);

    /// <summary>
    /// Enriches an LLM analysis with additional insights
    /// </summary>
    /// <param name="llmAnalysis">The LLM analysis to enrich</param>
    /// <returns>The enriched analysis</returns>
    Task<LLMAnalysisResult> EnrichLLMAnalysisAsync(LLMAnalysisResult llmAnalysis);

    /// <summary>
    /// Analyzes remediation options for an error
    /// </summary>
    /// <param name="errorContext">The error context</param>
    /// <returns>The remediation analysis</returns>
    Task<RuntimeErrorSage.Domain.Models.Remediation.RemediationAnalysis> AnalyzeRemediationAsync(ErrorContext errorContext);

    /// <summary>
    /// Analyzes the impact of an error
    /// </summary>
    /// <param name="errorContext">The error context</param>
    /// <returns>The impact analysis result</returns>
    Task<RuntimeErrorSage.Domain.Models.Graph.ImpactAnalysisResult> AnalyzeImpactAsync(ErrorContext errorContext);

    /// <summary>
    /// Analyzes the dependency graph for an error
    /// </summary>
    /// <param name="errorContext">The error context</param>
    /// <returns>The graph analysis result</returns>
    Task<RuntimeErrorSage.Domain.Models.Graph.GraphAnalysisResult> AnalyzeGraphAsync(ErrorContext errorContext);
} 
