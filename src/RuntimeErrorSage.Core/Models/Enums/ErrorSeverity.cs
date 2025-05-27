namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the severity level of an error.
/// </summary>
public enum ErrorSeverity
{
    /// <summary>
    /// The severity is unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The error is verbose.
    /// </summary>
    Verbose = 1,

    /// <summary>
    /// The error is informational.
    /// </summary>
    Information = 2,

    /// <summary>
    /// The error is a warning.
    /// </summary>
    Warning = 3,

    /// <summary>
    /// The error is an error.
    /// </summary>
    Error = 4,

    /// <summary>
    /// The error is critical.
    /// </summary>
    Critical = 5,

    /// <summary>
    /// The error is fatal.
    /// </summary>
    Fatal = 6
} 