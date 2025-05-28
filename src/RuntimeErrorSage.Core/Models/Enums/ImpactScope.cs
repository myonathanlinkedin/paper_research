namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the scope of an impact.
/// </summary>
public enum ImpactScope
{
    /// <summary>
    /// Unknown scope.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Local scope affecting only a single component.
    /// </summary>
    Local = 1,

    /// <summary>
    /// Component scope affecting a specific component and its direct dependencies.
    /// </summary>
    Component = 2,

    /// <summary>
    /// Service scope affecting an entire service.
    /// </summary>
    Service = 3,

    /// <summary>
    /// System scope affecting the entire system.
    /// </summary>
    System = 4,

    /// <summary>
    /// Global scope affecting multiple systems or the entire platform.
    /// </summary>
    Global = 5
} 