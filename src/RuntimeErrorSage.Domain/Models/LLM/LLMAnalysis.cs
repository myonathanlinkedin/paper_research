using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.LLM;

/// <summary>
/// Represents an analysis result from the LLM.
/// </summary>
public class LLMAnalysis
{
    /// <summary>
    /// Gets or sets the unique identifier of the error being analyzed.
    /// </summary>
    public string ErrorId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the correlation identifier for tracking the analysis.
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of error being analyzed.
    /// </summary>
    public string ErrorType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the component where the error occurred.
    /// </summary>
    public string Component { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the service where the error occurred.
    /// </summary>
    public string Service { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the root cause of the error.
    /// </summary>
    public string RootCause { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of suggestions for remediation.
    /// </summary>
    public List<LLMSuggestion> Suggestions { get; set; } = new();

    /// <summary>
    /// Gets or sets the confidence score of the analysis.
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets the severity of the error.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the analysis was performed.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
} 
