using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.LLM;

/// <summary>
/// Interface for LLM analysis results.
/// </summary>
public interface ILLMAnalysis
{
    /// <summary>
    /// Gets or sets the unique identifier of the error being analyzed.
    /// </summary>
    string ErrorId { get; set; }

    /// <summary>
    /// Gets or sets the correlation identifier for tracking the analysis.
    /// </summary>
    string CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the type of error being analyzed.
    /// </summary>
    string ErrorType { get; set; }

    /// <summary>
    /// Gets or sets the component where the error occurred.
    /// </summary>
    string Component { get; set; }

    /// <summary>
    /// Gets or sets the service where the error occurred.
    /// </summary>
    string Service { get; set; }

    /// <summary>
    /// Gets or sets the root cause of the error.
    /// </summary>
    string RootCause { get; set; }

    /// <summary>
    /// Gets or sets the list of suggestions for remediation.
    /// </summary>
    List<LLMSuggestion> Suggestions { get; set; }

    /// <summary>
    /// Gets or sets the confidence score of the analysis.
    /// </summary>
    double Confidence { get; set; }

    /// <summary>
    /// Gets or sets the severity of the error.
    /// </summary>
    ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the analysis was performed.
    /// </summary>
    DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the explanations for each remediation strategy.
    /// </summary>
    IDictionary<string, string> StrategyExplanations { get; set; }

    /// <summary>
    /// Gets or sets the scores for each remediation strategy.
    /// </summary>
    IDictionary<string, double> StrategyScores { get; set; }

    /// <summary>
    /// Gets or sets the detailed analysis text.
    /// </summary>
    string Analysis { get; set; }

    /// <summary>
    /// Gets or sets the recommended approach for remediation.
    /// </summary>
    string Approach { get; set; }

    /// <summary>
    /// Gets or sets whether the analysis is valid.
    /// </summary>
    bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the error message if the analysis is invalid.
    /// </summary>
    string ErrorMessage { get; set; }
} 
