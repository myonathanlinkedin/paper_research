namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Defines categories for validation operations.
/// </summary>
public enum ValidationCategory
{
    /// <summary>
    /// Input validation category.
    /// </summary>
    Input = 0,

    /// <summary>
    /// Business logic validation category.
    /// </summary>
    BusinessLogic = 1,

    /// <summary>
    /// Data integrity validation category.
    /// </summary>
    DataIntegrity = 2,

    /// <summary>
    /// Security validation category.
    /// </summary>
    Security = 3,

    /// <summary>
    /// Performance validation category.
    /// </summary>
    Performance = 4,

    /// <summary>
    /// Configuration validation category.
    /// </summary>
    Configuration = 5,

    /// <summary>
    /// Dependency validation category.
    /// </summary>
    Dependency = 6,

    /// <summary>
    /// Resource validation category.
    /// </summary>
    Resource = 7,

    /// <summary>
    /// State validation category.
    /// </summary>
    State = 8,

    /// <summary>
    /// Custom validation category.
    /// </summary>
    Custom = 9
} 
