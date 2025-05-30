namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the severity level of an error or event.
/// </summary>
public enum SeverityLevel
{
    /// <summary>
    /// Unknown severity.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// No severity.
    /// </summary>
    None = 1,

    /// <summary>
    /// Informational message.
    /// </summary>
    Info = 2,

    /// <summary>
    /// Success message.
    /// </summary>
    Success = 3,

    /// <summary>
    /// Low severity.
    /// </summary>
    Low = 4,

    /// <summary>
    /// Warning severity.
    /// </summary>
    Warning = 5,

    /// <summary>
    /// Medium severity.
    /// </summary>
    Medium = 6,

    /// <summary>
    /// Error severity.
    /// </summary>
    Error = 7,

    /// <summary>
    /// High severity.
    /// </summary>
    High = 8,

    /// <summary>
    /// Critical severity.
    /// </summary>
    Critical = 9,

    /// <summary>
    /// Fatal severity.
    /// </summary>
    Fatal = 10
} 
