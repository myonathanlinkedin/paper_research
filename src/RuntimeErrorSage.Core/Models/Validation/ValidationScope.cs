namespace RuntimeErrorSage.Core.Models.Validation;

/// <summary>
/// Defines the scope of validation.
/// </summary>
public enum ValidationScope
{
    /// <summary>
    /// System-wide validation scope.
    /// </summary>
    System,

    /// <summary>
    /// Application-wide validation scope.
    /// </summary>
    Application,

    /// <summary>
    /// Component-level validation scope.
    /// </summary>
    Component,

    /// <summary>
    /// Module-level validation scope.
    /// </summary>
    Module,

    /// <summary>
    /// Function-level validation scope.
    /// </summary>
    Function,

    /// <summary>
    /// Line-level validation scope.
    /// </summary>
    Line,

    /// <summary>
    /// Unknown validation scope.
    /// </summary>
    Unknown
} 