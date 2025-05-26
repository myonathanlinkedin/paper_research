namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents the status of an analysis operation.
/// </summary>
public enum AnalysisStatus
{
    /// <summary>
    /// The analysis has not started.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// The analysis is in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The analysis has completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// The analysis has failed.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// The analysis has been cancelled.
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// The analysis is waiting for additional data.
    /// </summary>
    WaitingForData = 5,

    /// <summary>
    /// The analysis has been partially completed.
    /// </summary>
    PartiallyCompleted = 6,

    /// <summary>
    /// The analysis has timed out.
    /// </summary>
    TimedOut = 7
} 