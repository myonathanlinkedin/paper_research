using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.LLM;

/// <summary>
/// Represents the result of an LLM analysis.
/// </summary>
public class LLMAnalysisResult
{
    /// <summary>
    /// Gets or sets the start time of the analysis.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the analysis.
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Gets or sets the correlation identifier for tracking the analysis.
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the analysis.
    /// </summary>
    public AnalysisStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the root cause of the error.
    /// </summary>
    public string RootCause { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the confidence score of the analysis.
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets the severity of the error.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the list of suggestions for remediation.
    /// </summary>
    public List<LLMSuggestion> Suggestions { get; set; } = new();

    /// <summary>
    /// Gets or sets the timestamp when the analysis result was created.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
} 
