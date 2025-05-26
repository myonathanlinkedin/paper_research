namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the execution status of a remediation step.
    /// </summary>
    public enum StepExecutionStatus
    {
        /// <summary>
        /// The step has not started execution.
        /// </summary>
        NotStarted,

        /// <summary>
        /// The step is in progress.
        /// </summary>
        InProgress,

        /// <summary>
        /// The step has completed execution.
        /// </summary>
        Completed,

        /// <summary>
        /// The step has failed execution.
        /// </summary>
        Failed,

        /// <summary>
        /// The step has timed out.
        /// </summary>
        Timeout,

        /// <summary>
        /// The step has been skipped.
        /// </summary>
        Skipped,

        /// <summary>
        /// The step is being retried.
        /// </summary>
        Retrying,

        /// <summary>
        /// The step is being rolled back.
        /// </summary>
        RollingBack,

        /// <summary>
        /// The step has been rolled back.
        /// </summary>
        RolledBack,

        /// <summary>
        /// The step has been cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// The step is waiting for approval.
        /// </summary>
        WaitingForApproval,

        /// <summary>
        /// The step has been approved.
        /// </summary>
        Approved,

        /// <summary>
        /// The step has been rejected.
        /// </summary>
        Rejected
    }
} 