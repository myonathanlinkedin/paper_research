namespace RuntimeErrorSage.Core.Models.Common;

/// <summary>
/// Defines the scope of impact.
/// </summary>
public enum ImpactScope
{
    /// <summary>
    /// Global impact scope.
    /// </summary>
    Global,

    /// <summary>
    /// Service impact scope.
    /// </summary>
    Service,

    /// <summary>
    /// Component impact scope.
    /// </summary>
    Component,

    /// <summary>
    /// Operation impact scope.
    /// </summary>
    Operation,

    /// <summary>
    /// Local impact scope.
    /// </summary>
    Local
} 