namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the types of nodes in a dependency graph.
/// </summary>
public enum GraphNodeType
{
    /// <summary>
    /// Unknown node type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Component node.
    /// </summary>
    Component = 1,

    /// <summary>
    /// Service node.
    /// </summary>
    Service = 2,

    /// <summary>
    /// Module node.
    /// </summary>
    Module = 3,

    /// <summary>
    /// Class node.
    /// </summary>
    Class = 4,

    /// <summary>
    /// Method node.
    /// </summary>
    Method = 5,

    /// <summary>
    /// Variable node.
    /// </summary>
    Variable = 6,

    /// <summary>
    /// Resource node.
    /// </summary>
    Resource = 7,

    /// <summary>
    /// Configuration node.
    /// </summary>
    Configuration = 8,

    /// <summary>
    /// Error node.
    /// </summary>
    Error = 9,

    /// <summary>
    /// Root cause node.
    /// </summary>
    RootCause = 10
} 