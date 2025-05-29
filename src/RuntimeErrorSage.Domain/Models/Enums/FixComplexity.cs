namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Defines the complexity levels for error fixes.
/// </summary>
public enum FixComplexity
{
    /// <summary>
    /// Unknown complexity.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Simple fix.
    /// </summary>
    Simple = 1,

    /// <summary>
    /// Moderate complexity fix.
    /// </summary>
    Moderate = 2,

    /// <summary>
    /// Complex fix.
    /// </summary>
    Complex = 3,

    /// <summary>
    /// Very complex fix.
    /// </summary>
    VeryComplex = 4,

    /// <summary>
    /// Extremely complex fix.
    /// </summary>
    ExtremelyComplex = 5
} 
