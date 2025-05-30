namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the status of an MCP (Model Control Protocol) connection.
/// </summary>
public enum MCPConnectionStatusEnum
{
    /// <summary>
    /// The connection is disconnected.
    /// </summary>
    Disconnected = 0,

    /// <summary>
    /// The connection is connecting.
    /// </summary>
    Connecting = 1,

    /// <summary>
    /// The connection is connected.
    /// </summary>
    Connected = 2,

    /// <summary>
    /// The connection is reconnecting.
    /// </summary>
    Reconnecting = 3,

    /// <summary>
    /// The connection has failed.
    /// </summary>
    Failed = 4,

    /// <summary>
    /// The connection status is unknown.
    /// </summary>
    Unknown = 5
} 
