namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Defines levels for validation operations.
/// </summary>
public enum ValidationLevel
{
    /// <summary>
    /// Basic validation level.
    /// </summary>
    Basic = 0,

    /// <summary>
    /// Standard validation level.
    /// </summary>
    Standard = 1,

    /// <summary>
    /// Advanced validation level.
    /// </summary>
    Advanced = 2,

    /// <summary>
    /// Strict validation level.
    /// </summary>
    Strict = 3,

    /// <summary>
    /// Custom validation level.
    /// </summary>
    Custom = 4
} 
