namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Enumeration of remediation states.
    /// </summary>
    public enum RemediationStateEnum
    {
        /// <summary>
        /// The remediation is not started.
        /// </summary>
        NotStarted = 0,

        /// <summary>
        /// The remediation is in progress.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// The remediation is completed.
        /// </summary>
        Completed = 2,

        /// <summary>
        /// The remediation has failed.
        /// </summary>
        Failed = 3,

        /// <summary>
        /// The remediation is pending.
        /// </summary>
        Pending = 4,

        /// <summary>
        /// The remediation has been rolled back.
        /// </summary>
        RolledBack = 5,

        /// <summary>
        /// The remediation rollback has failed.
        /// </summary>
        RollbackFailed = 6
    }
} 