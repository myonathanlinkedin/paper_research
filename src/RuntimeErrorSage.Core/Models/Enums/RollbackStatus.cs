namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the status of a rollback operation.
/// </summary>
public enum RollbackStatus
{
    /// <summary>
    /// The rollback has not been started.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// The rollback is in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The rollback has completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// The rollback has failed.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// The rollback is not required or applicable.
    /// </summary>
    NotRequired = 4,

    /// <summary>
    /// The rollback has been cancelled.
    /// </summary>
    Cancelled = 5
} 