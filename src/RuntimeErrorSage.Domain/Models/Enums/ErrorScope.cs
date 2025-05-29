using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Defines the scope of an error.
/// </summary>
public enum ErrorScope
{
    /// <summary>
    /// Unknown scope.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// System level scope.
    /// </summary>
    System = 1,

    /// <summary>
    /// Application level scope.
    /// </summary>
    Application = 2,

    /// <summary>
    /// Service level scope.
    /// </summary>
    Service = 3,

    /// <summary>
    /// Module level scope.
    /// </summary>
    Module = 4,

    /// <summary>
    /// Component level scope.
    /// </summary>
    Component = 5,

    /// <summary>
    /// Method level scope.
    /// </summary>
    Method = 6,

    /// <summary>
    /// Statement level scope.
    /// </summary>
    Statement = 7,

    /// <summary>
    /// Expression level scope.
    /// </summary>
    Expression = 8
} 






