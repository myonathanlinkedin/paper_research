namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the scope of impact for a graph node.
/// </summary>
public enum ImpactScope
{
    /// <summary>
    /// No impact.
    /// </summary>
    None = 0,

    /// <summary>
    /// Local impact affecting only the immediate component.
    /// </summary>
    Local = 1,

    /// <summary>
    /// Module impact affecting the containing module.
    /// </summary>
    Module = 2,

    /// <summary>
    /// Service impact affecting the entire service.
    /// </summary>
    Service = 3,

    /// <summary>
    /// System impact affecting multiple services.
    /// </summary>
    System = 4,

    /// <summary>
    /// Global impact affecting the entire application.
    /// </summary>
    Global = 5
} 