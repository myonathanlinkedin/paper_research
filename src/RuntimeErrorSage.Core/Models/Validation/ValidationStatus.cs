namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Defines the possible validation statuses.
/// </summary>
public enum ValidationStatus
{
    /// <summary>
    /// Validation passed successfully.
    /// </summary>
    Passed,

    /// <summary>
    /// Validation failed.
    /// </summary>
    Failed,

    /// <summary>
    /// Validation resulted in a warning.
    /// </summary>
    Warning,

    /// <summary>
    /// Validation was skipped.
    /// </summary>
    Skipped,

    /// <summary>
    /// Validation is in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// Validation status is unknown.
    /// </summary>
    Unknown
} 