using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Defines the types of edges in a dependency graph.
/// </summary>
public enum GraphEdgeType
{
    /// <summary>
    /// Unknown edge type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Calls relationship.
    /// </summary>
    Calls = 1,

    /// <summary>
    /// Depends on relationship.
    /// </summary>
    DependsOn = 2,

    /// <summary>
    /// Contains relationship.
    /// </summary>
    Contains = 3,

    /// <summary>
    /// References relationship.
    /// </summary>
    References = 4,

    /// <summary>
    /// Inherits from relationship.
    /// </summary>
    InheritsFrom = 5,

    /// <summary>
    /// Implements relationship.
    /// </summary>
    Implements = 6,

    /// <summary>
    /// Causes relationship.
    /// </summary>
    Causes = 7,

    /// <summary>
    /// Affects relationship.
    /// </summary>
    Affects = 8,

    /// <summary>
    /// Correlates with relationship.
    /// </summary>
    CorrelatesWith = 9,

    /// <summary>
    /// Precedes relationship.
    /// </summary>
    Precedes = 10,

    /// <summary>
    /// Follows relationship.
    /// </summary>
    Follows = 11
} 






