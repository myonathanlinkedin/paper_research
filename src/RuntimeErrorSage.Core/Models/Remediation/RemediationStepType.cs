namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents the type of remediation step.
/// </summary>
public enum RemediationStepType
{
    /// <summary>
    /// Validation step to check conditions.
    /// </summary>
    Validation = 0,

    /// <summary>
    /// Preparation step to set up the environment.
    /// </summary>
    Preparation = 1,

    /// <summary>
    /// Execution step to perform the remediation.
    /// </summary>
    Execution = 2,

    /// <summary>
    /// Verification step to confirm the remediation.
    /// </summary>
    Verification = 3,

    /// <summary>
    /// Cleanup step to restore the environment.
    /// </summary>
    Cleanup = 4,

    /// <summary>
    /// Rollback step to undo changes.
    /// </summary>
    Rollback = 5,

    /// <summary>
    /// Notification step to inform stakeholders.
    /// </summary>
    Notification = 6,

    /// <summary>
    /// Documentation step to record changes.
    /// </summary>
    Documentation = 7
} 