namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Defines scopes for validation operations.
/// </summary>
public enum ValidationScope
{
    /// <summary>
    /// Property level scope.
    /// </summary>
    Property = 0,

    /// <summary>
    /// Object level scope.
    /// </summary>
    Object = 1,

    /// <summary>
    /// Collection level scope.
    /// </summary>
    Collection = 2,

    /// <summary>
    /// Component level scope.
    /// </summary>
    Component = 3,

    /// <summary>
    /// Module level scope.
    /// </summary>
    Module = 4,

    /// <summary>
    /// Service level scope.
    /// </summary>
    Service = 5,

    /// <summary>
    /// System level scope.
    /// </summary>
    System = 6,

    /// <summary>
    /// Cross-system scope.
    /// </summary>
    CrossSystem = 7
} 
