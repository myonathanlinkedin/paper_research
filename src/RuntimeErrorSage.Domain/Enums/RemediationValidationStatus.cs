namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the validation status of a remediation action.
/// </summary>
public enum RemediationValidationStatus
{
    /// <summary>
    /// Validation not required.
    /// </summary>
    NotRequired,

    /// <summary>
    /// Waiting for validation.
    /// </summary>
    Pending,

    /// <summary>
    /// Validation in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// Validation passed.
    /// </summary>
    Passed,

    /// <summary>
    /// Validation failed.
    /// </summary>
    Failed,

    /// <summary>
    /// Validation status is unknown.
    /// </summary>
    Unknown
} 
