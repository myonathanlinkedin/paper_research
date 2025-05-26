namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Defines types of validation operations.
/// </summary>
public enum ValidationType
{
    /// <summary>
    /// Data validation.
    /// </summary>
    Data = 0,

    /// <summary>
    /// Business rule validation.
    /// </summary>
    BusinessRule = 1,

    /// <summary>
    /// Security validation.
    /// </summary>
    Security = 2,

    /// <summary>
    /// Performance validation.
    /// </summary>
    Performance = 3,

    /// <summary>
    /// Configuration validation.
    /// </summary>
    Configuration = 4,

    /// <summary>
    /// Dependency validation.
    /// </summary>
    Dependency = 5,

    /// <summary>
    /// Resource validation.
    /// </summary>
    Resource = 6,

    /// <summary>
    /// State validation.
    /// </summary>
    State = 7,

    /// <summary>
    /// Custom validation type.
    /// </summary>
    Custom = 8
} 
