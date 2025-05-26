namespace RuntimeErrorSage.Core.Models.Error.Enums;

/// <summary>
/// Represents the status of an error analysis.
/// </summary>
public enum AnalysisStatus
{
    /// <summary>
    /// The analysis is pending.
    /// </summary>
    Pending,

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
    /// The analysis has timed out.
    /// </summary>
    TimedOut,

    /// <summary>
    /// The analysis status is unknown.
    /// </summary>
    Unknown
} 