namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the severity level of an impact.
/// </summary>
public enum ImpactSeverity
{
    /// <summary>
    /// Unknown severity level.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// No severity level specified.
    /// </summary>
    None = 1,

    /// <summary>
    /// Informational severity level. No impact on functionality.
    /// </summary>
    Info = 2,

    /// <summary>
    /// Success level - operation passed.
    /// </summary>
    Success = 3,

    /// <summary>
    /// Low severity level. Minor issues or warnings.
    /// </summary>
    Low = 4,

    /// <summary>
    /// Warning level - potential issues found.
    /// </summary>
    Warning = 5,

    /// <summary>
    /// Medium severity level. Functionality is impaired but not blocked.
    /// </summary>
    Medium = 6,

    /// <summary>
    /// Error level - issues found that must be fixed.
    /// </summary>
    Error = 7,

    /// <summary>
    /// High severity level. Major functionality is affected.
    /// </summary>
    High = 8,

    /// <summary>
    /// Critical severity level. System is unusable or data is at risk.
    /// </summary>
    Critical = 9,

    /// <summary>
    /// Fatal severity level. Unrecoverable issues found.
    /// </summary>
    Fatal = 10
} 
