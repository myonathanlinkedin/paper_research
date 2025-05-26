namespace RuntimeErrorSage.Core.Models.Common;

/// <summary>
/// Represents the status of an analysis operation.
/// </summary>
public enum AnalysisStatus
{
    /// <summary>
    /// The analysis has not started.
    /// </summary>
    NotStarted,

    /// <summary>
    /// The analysis is in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// The analysis has completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// The analysis has failed.
    /// </summary>
    Failed,

    /// <summary>
    /// The analysis has been cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// The analysis is waiting for additional data.
    /// </summary>
    WaitingForData,

    /// <summary>
    /// The analysis has been partially completed.
    /// </summary>
    PartiallyCompleted,

    /// <summary>
    /// The analysis has timed out.
    /// </summary>
    TimedOut
} 