using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Analysis.Interfaces;

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
    Task<ErrorAnalysis> AnalyzeAsync(Error error);

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
} 