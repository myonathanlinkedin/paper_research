using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents a runtime error in the system.
/// </summary>
public class RuntimeError
{
    /// <summary>
    /// Gets or sets the unique identifier for this error.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the stack trace.
    /// </summary>
    public string StackTrace { get; set; }

    /// <summary>
    /// Gets or sets the source of the error.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets the type of the error.
    /// </summary>
    public string ErrorType { get; set; }

    /// <summary>
    /// Gets or sets the severity of the error.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the error occurred.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the component where the error occurred.
    /// </summary>
    public string Component { get; set; }

    /// <summary>
    /// Gets or sets the method where the error occurred.
    /// </summary>
    public string Method { get; set; }

    /// <summary>
    /// Gets or sets the line number where the error occurred.
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// Gets or sets the file where the error occurred.
    /// </summary>
    public string File { get; set; }

    /// <summary>
    /// Gets or sets additional error data.
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();

    /// <summary>
    /// Gets or sets the inner error if any.
    /// </summary>
    public RuntimeError InnerError { get; set; }

    /// <summary>
    /// Gets or sets the correlation ID for tracing purposes.
    /// </summary>
    public string CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the environment where the error occurred.
    /// </summary>
    public string Environment { get; set; }

    /// <summary>
    /// Gets or sets whether this is a recurring error.
    /// </summary>
    public bool IsRecurring { get; set; }

    /// <summary>
    /// Gets or sets the frequency of occurrence if recurring.
    /// </summary>
    public int OccurrenceCount { get; set; }

    /// <summary>
    /// Gets or sets the first occurrence timestamp if recurring.
    /// </summary>
    public DateTime? FirstOccurrence { get; set; }

    /// <summary>
    /// Gets or sets the error category.
    /// </summary>
    public ErrorCategory Category { get; set; }

    /// <summary>
    /// Gets or sets the error impact level.
    /// </summary>
    public ImpactLevel Impact { get; set; }

    /// <summary>
    /// Gets or sets the error state.
    /// </summary>
    public ErrorState State { get; set; } = ErrorState.New;

    /// <summary>
    /// Gets or sets the list of tags associated with this error.
    /// </summary>
    public List<string> Tags { get; set; } = new();
}

/// <summary>
/// Represents the severity of an error.
/// </summary>
public enum ErrorSeverity
{
    Low,
    Medium,
    High,
    Critical
}

/// <summary>
/// Represents the category of an error.
/// </summary>
public enum ErrorCategory
{
    Runtime,
    Syntax,
    Logic,
    Resource,
    Security,
    Performance,
    Configuration,
    Network,
    Database,
    External,
    Unknown
}

/// <summary>
/// Represents the impact level of an error.
/// </summary>
public enum ImpactLevel
{
    None,
    Minimal,
    Moderate,
    Significant,
    Severe
}

/// <summary>
/// Represents the state of an error.
/// </summary>
public enum ErrorState
{
    New,
    Analyzing,
    Remediating,
    Resolved,
    Closed
} 