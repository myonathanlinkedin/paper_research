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
    /// Gets or sets the unique identifier for this analysis.
    /// </summary>
    public string AnalysisId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the analysis text.
    /// </summary>
    public string AnalysisText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the confidence score (0-1).
    /// </summary>
    public double ConfidenceScore { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the analysis.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the model used for analysis.
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the model version.
    /// </summary>
    public string ModelVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the analysis duration in milliseconds.
    /// </summary>
    public long AnalysisDuration { get; set; }

    /// <summary>
    /// Gets or sets the tokens used.
    /// </summary>
    public int TokensUsed { get; set; }

    /// <summary>
    /// Gets or sets the analysis metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the error message if analysis failed.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the analysis was successful.
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// Gets or sets the correlation ID.
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;
} 