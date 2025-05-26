using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents an error classification.
/// </summary>
public class ErrorClassification
{
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
    /// Gets or sets the confidence score.
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets additional classification details.
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new();
} 