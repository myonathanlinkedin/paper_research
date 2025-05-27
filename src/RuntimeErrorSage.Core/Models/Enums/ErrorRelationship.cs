namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the type of relationship between errors.
/// </summary>
public enum ErrorRelationship
{
    /// <summary>
    /// No relationship.
    /// </summary>
    None,

    /// <summary>
    /// Parent-child relationship.
    /// </summary>
    ParentChild,

    /// <summary>
    /// Sibling relationship.
    /// </summary>
    Sibling,

    /// <summary>
    /// Dependency relationship.
    /// </summary>
    Dependency,

    /// <summary>
    /// Correlation relationship.
    /// </summary>
    Correlation,

    /// <summary>
    /// Causation relationship.
    /// </summary>
    Causation,

    /// <summary>
    /// Temporal relationship.
    /// </summary>
    Temporal,

    /// <summary>
    /// Spatial relationship.
    /// </summary>
    Spatial,

    /// <summary>
    /// Logical relationship.
    /// </summary>
    Logical
} 