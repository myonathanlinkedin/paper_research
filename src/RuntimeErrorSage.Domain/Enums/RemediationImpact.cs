using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the impact level of a remediation operation.
/// </summary>
public enum RemediationImpactLevel
{
    /// <summary>
    /// No impact
    /// </summary>
    None = 0,

    /// <summary>
    /// Minimal impact - no significant effect
    /// </summary>
    Minimal = 1,

    /// <summary>
    /// Low impact - minor functionality affected
    /// </summary>
    Low = 2,

    /// <summary>
    /// Medium impact - some functionality affected
    /// </summary>
    Medium = 3,

    /// <summary>
    /// High impact - major functionality affected
    /// </summary>
    High = 4,

    /// <summary>
    /// Critical impact - system is unusable
    /// </summary>
    Critical = 5,

    /// <summary>
    /// Impact level is unknown
    /// </summary>
    Unknown = 6
} 






