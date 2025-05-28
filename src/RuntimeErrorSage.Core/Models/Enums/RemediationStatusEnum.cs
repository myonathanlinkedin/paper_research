namespace RuntimeErrorSage.Core.Models.Enums
{
    /// <summary>
    /// Defines the status of a remediation action.
    /// </summary>
    public enum RemediationStatusEnum
    {
        /// <summary>
        /// The remediation has not yet started.
        /// </summary>
        NotStarted = 0,

        /// <summary>
        /// The remediation is created but not yet started.
        /// </summary>
        Created = 1,

        /// <summary>
        /// The remediation is in progress.
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// The remediation has been completed successfully.
        /// </summary>
        Completed = 3,

        /// <summary>
        /// The remediation has failed.
        /// </summary>
        Failed = 4,

        /// <summary>
        /// The remediation has been rolled back.
        /// </summary>
        RolledBack = 5,

        /// <summary>
        /// The remediation is pending validation.
        /// </summary>
        PendingValidation = 6,

        /// <summary>
        /// The remediation is waiting for approval.
        /// </summary>
        WaitingForApproval = 7
    }
} 