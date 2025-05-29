using System;

namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the status of a validation operation.
/// </summary>
public enum ValidationStatus
{
    /// <summary>
    /// Unknown validation status.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Pending validation status.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// In progress validation status.
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// Completed validation status.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Failed validation status.
    /// </summary>
    Failed = 4,

    /// <summary>
    /// Skipped validation status.
    /// </summary>
    Skipped = 5,

    /// <summary>
    /// Cancelled validation status.
    /// </summary>
    Cancelled = 6,

    /// <summary>
    /// The validation has timed out.
    /// </summary>
    TimedOut
} 

