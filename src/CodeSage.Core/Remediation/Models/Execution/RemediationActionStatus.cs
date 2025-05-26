namespace CodeSage.Core.Remediation.Models.Execution
{
    /// <summary>
    /// Represents the status of a remediation action execution.
    /// </summary>
    public enum RemediationActionStatus
    {
        /// <summary>
        /// The action is pending execution.
        /// </summary>
        Pending,

        /// <summary>
        /// The action is currently running.
        /// </summary>
        Running,

        /// <summary>
        /// The action has been completed successfully.
        /// </summary>
        Completed,

        /// <summary>
        /// The action has failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The action has been cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// The action is waiting for approval.
        /// </summary>
        WaitingForApproval,

        /// <summary>
        /// The action was skipped.
        /// </summary>
        Skipped
    }
} 