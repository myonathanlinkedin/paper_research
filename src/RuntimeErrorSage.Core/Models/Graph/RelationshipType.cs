namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents the type of relationship between nodes in a dependency graph.
/// </summary>
public enum RelationshipType
{
    /// <summary>
    /// A standard dependency relationship.
    /// </summary>
    Standard = 0,

    /// <summary>
    /// A critical dependency that must be satisfied.
    /// </summary>
    Critical = 1,

    /// <summary>
    /// An optional dependency that enhances functionality.
    /// </summary>
    Optional = 2,

    /// <summary>
    /// A transient dependency that may change.
    /// </summary>
    Transient = 3,

    /// <summary>
    /// A bidirectional dependency relationship.
    /// </summary>
    Bidirectional = 4,

    /// <summary>
    /// A composite dependency relationship.
    /// </summary>
    Composite = 5,

    /// <summary>
    /// A runtime dependency relationship.
    /// </summary>
    Runtime = 6,

    /// <summary>
    /// A development dependency relationship.
    /// </summary>
    Development = 7
} 