namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents the severity level of an impact.
/// </summary>
public enum ImpactSeverity
{
    /// <summary>
    /// No impact or negligible effect.
    /// </summary>
    None = 0,

    /// <summary>
    /// Low impact with minimal disruption.
    /// </summary>
    Low = 1,

    /// <summary>
    /// Medium impact with noticeable effects.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// High impact with significant disruption.
    /// </summary>
    High = 3,

    /// <summary>
    /// Critical impact with severe consequences.
    /// </summary>
    Critical = 4,

    /// <summary>
    /// Catastrophic impact with system-wide failure.
    /// </summary>
    Catastrophic = 5
} 