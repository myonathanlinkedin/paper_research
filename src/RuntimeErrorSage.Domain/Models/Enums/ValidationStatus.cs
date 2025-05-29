using System;

namespace RuntimeErrorSage.Model.Models.Enums;

/// <summary>
/// Represents the status of a validation operation.
/// </summary>
public enum ValidationStatus
{
    /// <summary>
    /// The validation is pending.
    /// </summary>
    Pending,

    /// <summary>
    /// The validation is in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// The validation has completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// The validation has failed.
    /// </summary>
    Failed,

    /// <summary>
    /// The validation has been cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// The validation has timed out.
    /// </summary>
    TimedOut,

    /// <summary>
    /// The validation has been skipped.
    /// </summary>
    Skipped
} 

