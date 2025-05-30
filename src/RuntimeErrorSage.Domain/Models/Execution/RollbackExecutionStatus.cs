namespace RuntimeErrorSage.Domain.Models.Execution;

/// <summary>
/// Represents the execution status of a rollback operation.
/// </summary>
public enum RollbackExecutionStatus
{
    /// <summary>
    /// The rollback status is unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The rollback is pending execution.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// The rollback is currently running.
    /// </summary>
    Running = 2,

    /// <summary>
    /// The rollback has completed successfully.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// The rollback has failed.
    /// </summary>
    Failed = 4,

    /// <summary>
    /// The rollback was cancelled.
    /// </summary>
    Cancelled = 5,

    /// <summary>
    /// The rollback completed with partial success.
    /// </summary>
    Partial = 6
} 
