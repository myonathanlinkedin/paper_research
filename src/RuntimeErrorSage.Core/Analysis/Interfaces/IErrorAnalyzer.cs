using System;
using System.Threading.Tasks;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.LLM;

namespace RuntimeErrorSage.Model.Analysis.Interfaces;

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
} 
