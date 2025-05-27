namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the validation status of an analysis.
/// </summary>
public enum AnalysisValidationStatus
{
    /// <summary>
    /// The analysis has not been validated.
    /// </summary>
    NotValidated = 0,

    /// <summary>
    /// The analysis is currently being validated.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The analysis has been validated successfully.
    /// </summary>
    Valid = 2,

    /// <summary>
    /// The analysis validation has failed.
    /// </summary>
    Invalid = 3,

    /// <summary>
    /// The analysis validation has been skipped.
    /// </summary>
    Skipped = 4,

    /// <summary>
    /// The analysis validation status is unknown.
    /// </summary>
    Unknown = 5
} 