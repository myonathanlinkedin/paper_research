namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the type of node in a dependency graph.
/// </summary>
public enum GraphNodeType
{
    /// <summary>
    /// Unknown node type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// A component node representing a service or module.
    /// </summary>
    Component = 1,

    /// <summary>
    /// An error node representing an error or exception.
    /// </summary>
    Error = 2,

    /// <summary>
    /// A resource node representing a system resource.
    /// </summary>
    Resource = 3,

    /// <summary>
    /// A data node representing data or a database.
    /// </summary>
    Data = 4,

    /// <summary>
    /// A user node representing a user or client.
    /// </summary>
    User = 5,

    /// <summary>
    /// An API node representing an API endpoint.
    /// </summary>
    Api = 6
} 