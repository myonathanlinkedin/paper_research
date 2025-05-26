namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Defines the types of validation.
/// </summary>
public enum ValidationType
{
    /// <summary>
    /// Static validation type.
    /// </summary>
    Static,

    /// <summary>
    /// Dynamic validation type.
    /// </summary>
    Dynamic,

    /// <summary>
    /// Hybrid validation type.
    /// </summary>
    Hybrid,

    /// <summary>
    /// Unknown validation type.
    /// </summary>
    Unknown
} 