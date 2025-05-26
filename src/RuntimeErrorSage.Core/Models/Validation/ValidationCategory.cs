namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Defines the categories of validation.
/// </summary>
public enum ValidationCategory
{
    /// <summary>
    /// Syntax validation category.
    /// </summary>
    Syntax,

    /// <summary>
    /// Semantic validation category.
    /// </summary>
    Semantic,

    /// <summary>
    /// Runtime validation category.
    /// </summary>
    Runtime,

    /// <summary>
    /// Security validation category.
    /// </summary>
    Security,

    /// <summary>
    /// Performance validation category.
    /// </summary>
    Performance,

    /// <summary>
    /// Resource validation category.
    /// </summary>
    Resource,

    /// <summary>
    /// Dependency validation category.
    /// </summary>
    Dependency,

    /// <summary>
    /// Configuration validation category.
    /// </summary>
    Configuration,

    /// <summary>
    /// Unknown validation category.
    /// </summary>
    Unknown
} 