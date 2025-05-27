using System;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Middleware;

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
    /// Gets or sets the error severity.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

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