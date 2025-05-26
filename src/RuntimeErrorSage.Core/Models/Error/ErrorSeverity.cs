namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Severity level of an error.
/// </summary>
public enum ErrorSeverity
{
    /// <summary>
    /// Debug level - for development and debugging purposes.
    /// </summary>
    Debug,

    /// <summary>
    /// Information level - for general information.
    /// </summary>
    Information,

    /// <summary>
    /// Warning level - for potential issues.
    /// </summary>
    Warning,

    /// <summary>
    /// Error level - for errors that can be handled.
    /// </summary>
    Error,

    /// <summary>
    /// Critical level - for errors that cannot be handled.
    /// </summary>
    Critical
} 