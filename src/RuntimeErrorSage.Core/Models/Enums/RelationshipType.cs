namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the type of relationship between components.
/// </summary>
public enum RelationshipType
{
    /// <summary>
    /// No relationship.
    /// </summary>
    None = 0,

    /// <summary>
    /// Direct dependency relationship.
    /// </summary>
    Dependency = 1,

    /// <summary>
    /// Parent-child relationship.
    /// </summary>
    ParentChild = 2,

    /// <summary>
    /// Peer relationship.
    /// </summary>
    Peer = 3,

    /// <summary>
    /// Service-to-service relationship.
    /// </summary>
    ServiceToService = 4,

    /// <summary>
    /// Component-to-component relationship.
    /// </summary>
    ComponentToComponent = 5,

    /// <summary>
    /// Error propagation relationship.
    /// </summary>
    ErrorPropagation = 6,

    /// <summary>
    /// Impact relationship.
    /// </summary>
    Impact = 7,

    /// <summary>
    /// Correlation relationship.
    /// </summary>
    Correlation = 8
} 