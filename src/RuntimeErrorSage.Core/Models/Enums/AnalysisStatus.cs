namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the status of an analysis operation.
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
    /// Analysis has completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Analysis has failed.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// Analysis has been cancelled.
    /// </summary>
    Cancelled = 4
} 