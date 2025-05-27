namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the severity level of an impact.
/// </summary>
public enum ImpactSeverity
{
    /// <summary>
    /// No impact.
    /// </summary>
    None = 0,

    /// <summary>
    /// Low severity impact.
    /// </summary>
    Low = 1,

    /// <summary>
    /// Medium severity impact.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// High severity impact.
    /// </summary>
    High = 3,

    /// <summary>
    /// Critical severity impact.
    /// </summary>
    Critical = 4,

    /// <summary>
    /// Catastrophic severity impact.
    /// </summary>
    Catastrophic = 5
} 