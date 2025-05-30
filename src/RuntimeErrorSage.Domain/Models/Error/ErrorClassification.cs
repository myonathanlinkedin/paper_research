using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Error;

/// <summary>
/// Represents the classification of an error.
/// </summary>
public class ErrorClassification
{
    /// <summary>
    /// Gets or sets the category of the error.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the subcategory of the error.
    /// </summary>
    public string Subcategory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string ErrorType { get; set; }

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
