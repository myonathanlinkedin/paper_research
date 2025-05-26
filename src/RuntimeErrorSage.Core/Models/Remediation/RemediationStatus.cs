namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Specifies the status of a remediation operation.
    /// </summary>
    public enum RemediationStatus
    {
        /// <summary>
        /// Unknown status.
        /// </summary>
        Unknown,

        /// <summary>
        /// Pending status.
        /// </summary>
        Pending,

        /// <summary>
        /// In progress status.
        /// </summary>
        InProgress,

        /// <summary>
        /// Completed status.
        /// </summary>
        Completed,

        /// <summary>
        /// Failed status.
        /// </summary>
        Failed,

        /// <summary>
        /// Cancelled status.
        /// </summary>
        Cancelled,

        /// <summary>
        /// Timed out status.
        /// </summary>
        TimedOut,

        /// <summary>
        /// Rolled back status.
        /// </summary>
        RolledBack
    }
} 