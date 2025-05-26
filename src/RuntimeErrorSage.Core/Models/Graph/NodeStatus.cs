namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents the status of a node in a dependency graph.
/// </summary>
public enum NodeStatus
{
    /// <summary>
    /// The node is healthy and operating normally.
    /// </summary>
    Healthy = 0,

    /// <summary>
    /// The node is experiencing minor issues but is still functional.
    /// </summary>
    Warning = 1,

    /// <summary>
    /// The node is experiencing degraded performance.
    /// </summary>
    Degraded = 2,

    /// <summary>
    /// The node has encountered errors but is still operational.
    /// </summary>
    Error = 3,

    /// <summary>
    /// The node has failed and is not operational.
    /// </summary>
    Failed = 4,

    /// <summary>
    /// The node's status is unknown.
    /// </summary>
    Unknown = 5,

    /// <summary>
    /// The node is offline or not accessible.
    /// </summary>
    Offline = 6,

    /// <summary>
    /// The node is in maintenance mode.
    /// </summary>
    Maintenance = 7
} 