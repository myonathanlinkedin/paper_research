namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the status of an analysis operation.
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
    Cancelled = 4,

    /// <summary>
    /// Analysis is waiting for additional input or resources.
    /// </summary>
    Waiting = 5,

    /// <summary>
    /// Analysis has timed out.
    /// </summary>
    TimedOut = 6,

    /// <summary>
    /// Analysis requires validation.
    /// </summary>
    NeedsValidation = 7,

    /// <summary>
    /// Analysis has been validated.
    /// </summary>
    Validated = 8,
    
    /// <summary>
    /// Analysis is pending.
    /// </summary>
    Pending = 9
} 
