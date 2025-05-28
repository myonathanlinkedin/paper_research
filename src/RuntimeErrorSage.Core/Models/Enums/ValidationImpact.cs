namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines impact levels for validation.
/// </summary>
public enum ValidationImpact
{
    /// <summary>
    /// Critical impact - system is unusable.
    /// </summary>
    Critical,

    /// <summary>
    /// High impact - major functionality is impacted.
    /// </summary>
    High,

    /// <summary>
    /// Medium impact - system is degraded but functional.
    /// </summary>
    Medium,

    /// <summary>
    /// Low impact - minor impact on functionality.
    /// </summary>
    Low,

    /// <summary>
    /// No impact - no functional impact.
    /// </summary>
    None
} 

