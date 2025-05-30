namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the possible states of a rollback operation.
    /// </summary>
    public enum RollbackState
    {
        /// <summary>
        /// No rollback state specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Rollback is pending.
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Rollback is in progress.
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// Rollback has completed successfully.
        /// </summary>
        Completed = 3,

        /// <summary>
        /// Rollback has failed.
        /// </summary>
        Failed = 4,

        /// <summary>
        /// Rollback has been cancelled.
        /// </summary>
        Cancelled = 5,

        /// <summary>
        /// Rollback is waiting for additional input.
        /// </summary>
        Waiting = 6,

        /// <summary>
        /// Rollback has timed out.
        /// </summary>
        TimedOut = 7,

        /// <summary>
        /// Rollback requires manual intervention.
        /// </summary>
        NeedsManualIntervention = 8,

        /// <summary>
        /// Rollback has been skipped.
        /// </summary>
        Skipped = 9
    }
} 