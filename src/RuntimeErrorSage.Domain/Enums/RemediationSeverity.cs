using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the severity level of a remediation action.
/// </summary>
public enum RemediationSeverity
{
    /// <summary>
    /// Unknown severity level.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// No severity - action has no impact.
    /// </summary>
    None = 1,

    /// <summary>
    /// Low severity - minimal impact.
    /// </summary>
    Low = 2,

    /// <summary>
    /// Medium severity - moderate impact.
    /// </summary>
    Medium = 3,

    /// <summary>
    /// High severity - significant impact.
    /// </summary>
    High = 4,

    /// <summary>
    /// Critical severity - major impact.
    /// </summary>
    Critical = 5
} 





