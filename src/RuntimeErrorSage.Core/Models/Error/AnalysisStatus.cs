namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Represents the status of error analysis.
    /// </summary>
    public enum AnalysisStatus
    {
        /// <summary>
        /// Analysis has not started.
        /// </summary>
        NotStarted = 0,

        /// <summary>
        /// Analysis is in progress.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// Analysis completed successfully.
        /// </summary>
        Completed = 2,

        /// <summary>
        /// Analysis failed.
        /// </summary>
        Failed = 3,

        /// <summary>
        /// Analysis was cancelled.
        /// </summary>
        Cancelled = 4,

        /// <summary>
        /// Analysis is pending additional information.
        /// </summary>
        PendingInfo = 5,

        /// <summary>
        /// Analysis requires manual review.
        /// </summary>
        NeedsReview = 6,

        /// <summary>
        /// Analysis status is unknown.
        /// </summary>
        Unknown = 7
    }
} 