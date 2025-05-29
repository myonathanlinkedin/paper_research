namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the type of remediation action.
/// </summary>
public enum RemediationType
{
    /// <summary>
    /// Code modification remediation.
    /// </summary>
    CodeModification,

    /// <summary>
    /// Configuration change remediation.
    /// </summary>
    ConfigurationChange,

    /// <summary>
    /// Dependency update remediation.
    /// </summary>
    DependencyUpdate,

    /// <summary>
    /// Security patch remediation.
    /// </summary>
    SecurityPatch,

    /// <summary>
    /// Performance optimization remediation.
    /// </summary>
    PerformanceOptimization,

    /// <summary>
    /// Documentation update remediation.
    /// </summary>
    DocumentationUpdate,

    /// <summary>
    /// Test update remediation.
    /// </summary>
    TestUpdate,

    /// <summary>
    /// Other type of remediation.
    /// </summary>
    Other
} 
