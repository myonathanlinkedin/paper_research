namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the type of a remediation step.
/// </summary>
public enum RemediationStepType
{
    /// <summary>
    /// The step type is unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The step is a validation step.
    /// </summary>
    Validation = 1,

    /// <summary>
    /// The step is a preparation step.
    /// </summary>
    Preparation = 2,

    /// <summary>
    /// The step is an execution step.
    /// </summary>
    Execution = 3,

    /// <summary>
    /// The step is a verification step.
    /// </summary>
    Verification = 4,

    /// <summary>
    /// The step is a cleanup step.
    /// </summary>
    Cleanup = 5,

    /// <summary>
    /// The step is a rollback step.
    /// </summary>
    Rollback = 6
} 