namespace RuntimeErrorSage.Core.Models;

/// <summary>
/// Represents the severity level of an error.
/// </summary>
public enum ErrorSeverity
{
    /// <summary>
    /// Low severity - minor issues that don't affect core functionality
    /// </summary>
    Low = 0,

    /// <summary>
    /// Medium severity - issues that affect some functionality but have workarounds
    /// </summary>
    Medium = 1,

    /// <summary>
    /// High severity - issues that significantly impact functionality
    /// </summary>
    High = 2,

    /// <summary>
    /// Critical severity - issues that cause system failure or data loss
    /// </summary>
    Critical = 3
} 
