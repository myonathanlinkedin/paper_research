using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error.Enums;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents the result of an error analysis.
/// </summary>
public class ErrorAnalysisResult
{
    /// <summary>
    /// Gets or sets the error identifier.
    /// </summary>
    public string ErrorId { get; set; }

    /// <summary>
    /// Gets or sets the analysis timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the analysis status.
    /// </summary>
    public AnalysisStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string ErrorType { get; set; }

    /// <summary>
    /// Gets or sets the error category.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Gets or sets the error severity.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the error details.
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new();

    /// <summary>
    /// Gets or sets the fix complexity.
    /// </summary>
    public FixComplexity Complexity { get; set; }

    /// <summary>
    /// Gets or sets the priority level.
    /// </summary>
    public PreventionPriority Priority { get; set; }

    /// <summary>
    /// Gets or sets the estimated effectiveness.
    /// </summary>
    public double Effectiveness { get; set; }

    /// <summary>
    /// Gets or sets the implementation cost.
    /// </summary>
    public ImplementationCost Cost { get; set; }
} 