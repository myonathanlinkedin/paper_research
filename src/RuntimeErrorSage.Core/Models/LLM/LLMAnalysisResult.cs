using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.LLM;

/// <summary>
/// Represents the result of an LLM analysis operation.
/// </summary>
public class LLMAnalysisResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the analysis.
    /// </summary>
    public string AnalysisId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the model used for analysis.
    /// </summary>
    public string ModelName { get; set; }

    /// <summary>
    /// Gets or sets the model version.
    /// </summary>
    public string ModelVersion { get; set; }

    /// <summary>
    /// Gets or sets the analysis prompt.
    /// </summary>
    public string Prompt { get; set; }

    /// <summary>
    /// Gets or sets the analysis response.
    /// </summary>
    public string Response { get; set; }

    /// <summary>
    /// Gets or sets the confidence score (0-1).
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets the analysis start time.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the analysis end time.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the analysis duration in milliseconds.
    /// </summary>
    public double DurationMs => EndTime.HasValue ? (EndTime.Value - StartTime).TotalMilliseconds : 0;

    /// <summary>
    /// Gets or sets the analysis status.
    /// </summary>
    public AnalysisStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the analysis error message if any.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the analysis metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the validation results.
    /// </summary>
    public List<ValidationResult> ValidationResults { get; set; } = new();

    /// <summary>
    /// Gets or sets the analysis context.
    /// </summary>
    public LLMAnalysisContext Context { get; set; }

    public bool IsValid { get; set; }
    public string Analysis { get; set; } = string.Empty;
    public List<string> Suggestions { get; set; } = new();
    public ModelMetrics ModelMetrics { get; set; } = new();

    public void AddSuggestion(string suggestion)
    {
        Suggestions.Add(suggestion);
    }

    public void AddMetadata(string key, object value)
    {
        Metadata[key] = value;
    }

    public static LLMAnalysisResult Success(string analysis)
    {
        return new LLMAnalysisResult
        {
            IsValid = true,
            Analysis = analysis
        };
    }

    public static LLMAnalysisResult Failure(string error)
    {
        return new LLMAnalysisResult
        {
            IsValid = false,
            Analysis = error
        };
    }
}

/// <summary>
/// Represents the context for an LLM analysis.
/// </summary>
public class LLMAnalysisContext
{
    /// <summary>
    /// Gets or sets the unique identifier of the context.
    /// </summary>
    public string ContextId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the error context.
    /// </summary>
    public string ErrorContext { get; set; }

    /// <summary>
    /// Gets or sets the code context.
    /// </summary>
    public string CodeContext { get; set; }

    /// <summary>
    /// Gets or sets the runtime context.
    /// </summary>
    public string RuntimeContext { get; set; }

    /// <summary>
    /// Gets or sets the system context.
    /// </summary>
    public string SystemContext { get; set; }

    /// <summary>
    /// Gets or sets the context metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Defines the status of an analysis operation.
/// </summary>
public enum AnalysisStatus
{
    /// <summary>
    /// Analysis has been created.
    /// </summary>
    Created,

    /// <summary>
    /// Analysis is in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// Analysis has completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// Analysis has failed.
    /// </summary>
    Failed,

    /// <summary>
    /// Analysis was cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// Analysis status is unknown.
    /// </summary>
    Unknown
}

public class ModelMetrics
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string ModelName { get; set; } = string.Empty;
    public string ModelVersion { get; set; } = string.Empty;
    public double ResponseTime { get; set; }
    public int TokenCount { get; set; }
    public Dictionary<string, object> Properties { get; set; } = new();

    public void AddProperty(string key, object value)
    {
        Properties[key] = value;
    }
} 