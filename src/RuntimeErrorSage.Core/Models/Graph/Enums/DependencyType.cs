namespace RuntimeErrorSage.Core.Models.Graph.Enums;

/// <summary>
/// Specifies the dependency type.
/// </summary>
public enum DependencyType
{
    /// <summary>
    /// Runtime dependency.
    /// </summary>
    Runtime,

    /// <summary>
    /// Compile-time dependency.
    /// </summary>
    Compile,

    /// <summary>
    /// Development dependency.
    /// </summary>
    Development,

    /// <summary>
    /// Test dependency.
    /// </summary>
    Test
} 