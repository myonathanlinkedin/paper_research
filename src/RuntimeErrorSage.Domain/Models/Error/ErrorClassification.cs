using System.Collections.ObjectModel;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Error;

/// <summary>
/// Represents the classification of an error.
/// </summary>
public class ErrorClassification
{
    /// <summary>
    /// Gets or sets the category of the error.
    /// </summary>
    public string Category { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the subcategory of the error.
    /// </summary>
    public string Subcategory { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string ErrorType { get; }

    /// <summary>
    /// Gets or sets the error severity.
    /// </summary>
    public ErrorSeverity Severity { get; }

    /// <summary>
    /// Gets or sets the confidence score.
    /// </summary>
    public double Confidence { get; }

    /// <summary>
    /// Gets or sets additional classification details.
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new();
} 






