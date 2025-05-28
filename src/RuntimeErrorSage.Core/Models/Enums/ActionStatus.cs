namespace RuntimeErrorSage.Core.Models.Enums
{
    /// <summary>
    /// Defines the status of an action.
    /// </summary>
    public enum ActionStatus
    {
        /// <summary>
        /// The action is pending execution.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The action is in progress.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// The action has been completed successfully.
        /// </summary>
        Completed = 2,

        /// <summary>
        /// The action has failed.
        /// </summary>
        Failed = 3,

        /// <summary>
        /// The action has been rolled back.
        /// </summary>
        RolledBack = 4,

        /// <summary>
        /// The action has been skipped.
        /// </summary>
        Skipped = 5,

        /// <summary>
        /// The action has been cancelled.
        /// </summary>
        Cancelled = 6,

        /// <summary>
        /// The action is waiting for approval.
        /// </summary>
        WaitingForApproval = 7
    }
} 