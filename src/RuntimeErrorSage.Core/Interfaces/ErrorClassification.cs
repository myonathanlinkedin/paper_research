using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents an error classification.
/// </summary>
public class ErrorClassification
{
    public string ErrorType { get; set; }
    public string Category { get; set; }
    public ErrorSeverity Severity { get; set; }
    public double Confidence { get; set; }
    public Dictionary<string, object> Details { get; set; } = new();
} 