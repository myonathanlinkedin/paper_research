using System.Collections.ObjectModel;
using System;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Middleware;

/// <summary>
/// Represents an error response.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Gets or sets the error ID.
    /// </summary>
    public string ErrorId { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string Type { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the severity level of the error.
    /// </summary>
    public SeverityLevel Severity { get; }

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
    public DateTime Timestamp { get; }
} 






