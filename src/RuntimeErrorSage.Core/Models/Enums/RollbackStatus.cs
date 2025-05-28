namespace RuntimeErrorSage.Core.Models.Enums
{
    /// <summary>
    /// Defines the status of a rollback operation.
    /// </summary>
    public enum RollbackStatus
    {
        /// <summary>
        /// No rollback is required.
        /// </summary>
        NotRequired = 0,

        /// <summary>
        /// Rollback is pending.
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Rollback is in progress.
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// Rollback has been completed successfully.
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
        /// Rollback is waiting for approval.
        /// </summary>
        WaitingForApproval = 6
    }
} 