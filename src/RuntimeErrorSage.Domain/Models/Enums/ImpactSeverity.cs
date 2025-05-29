namespace RuntimeErrorSage.Model.Models.Enums;

/// <summary>
/// Defines the severity level of an impact.
/// </summary>
public enum ImpactSeverity
{
    /// <summary>
    /// Unknown severity level.
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Negligible impact.
    /// </summary>
    Negligible = 1,
    
    /// <summary>
    /// Minor impact.
    /// </summary>
    Minor = 2,
    
    /// <summary>
    /// Moderate impact.
    /// </summary>
    Moderate = 3,
    
    /// <summary>
    /// Major impact.
    /// </summary>
    Major = 4,
    
    /// <summary>
    /// Critical impact.
    /// </summary>
    Critical = 5,
    
    /// <summary>
    /// Catastrophic impact.
    /// </summary>
    Catastrophic = 6
} 
