using System;

namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents the status of a remediation operation.
/// </summary>
public class RemediationStatus
{
    /// <summary>
    /// Gets or sets the unique identifier of the status.
    /// </summary>
    public string StatusId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the current state of the remediation.
    /// </summary>
    public RemediationState State { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the status was last updated.
    /// </summary>
    public DateTimeOffset LastUpdated { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets or sets the progress percentage of the remediation (0-100).
    /// </summary>
    public int ProgressPercentage { get; set; }

    /// <summary>
    /// Gets or sets the current step being executed.
    /// </summary>
    public string CurrentStep { get; set; }

    /// <summary>
    /// Gets or sets the error message if the remediation failed.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the exception details if the remediation failed.
    /// </summary>
    public Exception Exception { get; set; }

    /// <summary>
    /// Gets or sets whether the remediation was rolled back.
    /// </summary>
    public bool WasRolledBack { get; set; }

    /// <summary>
    /// Gets or sets the metadata associated with the status.
    /// </summary>
    public System.Collections.Generic.Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Represents the state of a remediation operation.
/// </summary>
public enum RemediationState
{
    /// <summary>
    /// The remediation has not started yet.
    /// </summary>
    NotStarted,

    /// <summary>
    /// The remediation is currently running.
    /// </summary>
    Running,

    /// <summary>
    /// The remediation has completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// The remediation has failed.
    /// </summary>
    Failed,

    /// <summary>
    /// The remediation is being rolled back.
    /// </summary>
    RollingBack,

    /// <summary>
    /// The remediation has been rolled back.
    /// </summary>
    RolledBack,

    /// <summary>
    /// The remediation has been cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// The remediation is being validated.
    /// </summary>
    Validating,

    /// <summary>
    /// The remediation has been validated.
    /// </summary>
    Validated,

    /// <summary>
    /// The remediation validation has failed.
    /// </summary>
    ValidationFailed
} 