namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Defines the impact levels of validation results.
/// </summary>
public enum ValidationImpact
{
    /// <summary>
    /// Critical impact level.
    /// </summary>
    Critical,

    /// <summary>
    /// High impact level.
    /// </summary>
    High,

    /// <summary>
    /// Medium impact level.
    /// </summary>
    Medium,

    /// <summary>
    /// Low impact level.
    /// </summary>
    Low,

    /// <summary>
    /// No impact level.
    /// </summary>
    None
} 