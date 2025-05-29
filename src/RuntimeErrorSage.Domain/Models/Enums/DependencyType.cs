namespace RuntimeErrorSage.Model.Models.Enums;

/// <summary>
/// Represents the type of dependency between components.
/// </summary>
public enum DependencyType
{
    /// <summary>
    /// Runtime dependency between components.
    /// </summary>
    Runtime,

    /// <summary>
    /// Compile-time dependency between components.
    /// </summary>
    Compile,

    /// <summary>
    /// Development-time dependency between components.
    /// </summary>
    Development,

    /// <summary>
    /// Test-time dependency between components.
    /// </summary>
    Test
} 
