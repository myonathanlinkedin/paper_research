using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Represents the status of an analysis operation.
/// </summary>
public enum AnalysisStatus
{
    /// <summary>
    /// The analysis has not yet started.
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
    Cancelled = 4
} 






