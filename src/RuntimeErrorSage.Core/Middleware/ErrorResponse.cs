using System;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Model.Middleware;

/// <summary>
/// Represents an error response.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Gets or sets the error ID.
    /// </summary>
    public string ErrorId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the severity level of the error.
    /// </summary>
    public SeverityLevel Severity { get; set; }

    /// <summary>
    /// Gets or sets the error analysis.
    /// </summary>
    public string? Analysis { get; set; }

    /// <summary>
    /// Gets or sets the remediation information.
    /// </summary>
    public string? Remediation { get; set; }

    /// <summary>
    /// Gets or sets the error timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; }
} 
