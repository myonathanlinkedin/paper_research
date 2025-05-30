using System;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;

namespace RuntimeErrorSage.Application.Services.Interfaces;

/// <summary>
/// Service for analyzing exceptions and error contexts.
/// </summary>
public interface IErrorAnalysisService
{
    /// <summary>
    /// Analyzes an exception and its context to determine root cause and potential solutions.
    /// </summary>
    /// <param name="exception">The exception to analyze</param>
    /// <param name="context">The error context</param>
    /// <returns>Analysis result containing insights and recommendations</returns>
    Task<ErrorAnalysisResult> AnalyzeExceptionAsync(Exception exception, ErrorContext context);

    /// <summary>
    /// Analyzes an error context to determine patterns and relationships.
    /// </summary>
    /// <param name="context">The error context to analyze</param>
    /// <returns>Analysis result containing insights and recommendations</returns>
    Task<ErrorAnalysisResult> AnalyzeContextAsync(ErrorContext context);
} 
