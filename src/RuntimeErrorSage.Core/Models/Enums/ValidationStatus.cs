namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines validation status values.
/// </summary>
public enum ValidationStatus
{
    /// <summary>
    /// Validation has not been performed.
    /// </summary>
    NotValidated,

    /// <summary>
    /// Validation is in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// Validation passed successfully.
    /// </summary>
    Valid,

    /// <summary>
    /// Validation failed.
    /// </summary>
    Invalid,

    /// <summary>
    /// Validation was skipped.
    /// </summary>
    Skipped,

    /// <summary>
    /// Validation encountered an error.
    /// </summary>
    Error
} 
