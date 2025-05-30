namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the impact levels for validation operations.
/// </summary>
public enum ValidationImpact
{
    /// <summary>
    /// No impact.
    /// </summary>
    None = 0,

    /// <summary>
    /// Minimal impact on the system.
    /// </summary>
    Minimal = 1,

    /// <summary>
    /// Low impact on the system.
    /// </summary>
    Low = 2,

    /// <summary>
    /// Medium impact on the system.
    /// </summary>
    Medium = 3,

    /// <summary>
    /// High impact on the system.
    /// </summary>
    High = 4,

    /// <summary>
    /// Severe impact on the system.
    /// </summary>
    Severe = 5,

    /// <summary>
    /// Critical impact on the system.
    /// </summary>
    Critical = 6
} 

