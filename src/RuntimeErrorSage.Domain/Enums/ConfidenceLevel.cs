namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the confidence levels for remediation operations.
/// </summary>
public enum ConfidenceLevel
{
    /// <summary>
    /// No confidence level specified.
    /// </summary>
    None = 0,

    /// <summary>
    /// Very low confidence level.
    /// </summary>
    VeryLow = 1,

    /// <summary>
    /// Low confidence level.
    /// </summary>
    Low = 2,

    /// <summary>
    /// Medium confidence level.
    /// </summary>
    Medium = 3,

    /// <summary>
    /// High confidence level.
    /// </summary>
    High = 4,

    /// <summary>
    /// Very high confidence level.
    /// </summary>
    VeryHigh = 5,

    /// <summary>
    /// Certain confidence level.
    /// </summary>
    Certain = 6
} 
