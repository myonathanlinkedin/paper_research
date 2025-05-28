namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the validation status of an analysis.
/// </summary>
public enum AnalysisValidationStatus
{
    /// <summary>
    /// The analysis is valid.
    /// </summary>
    Valid = 0,
    
    /// <summary>
    /// The analysis is invalid.
    /// </summary>
    Invalid = 1,
    
    /// <summary>
    /// The analysis has warnings.
    /// </summary>
    Warning = 2,
    
    /// <summary>
    /// The analysis status is unknown.
    /// </summary>
    Unknown = 3
} 
