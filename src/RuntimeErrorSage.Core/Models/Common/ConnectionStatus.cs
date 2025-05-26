using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Common;

/// <summary>
/// Represents the connection status of a service or component.
/// </summary>
public class ConnectionStatus
{
    /// <summary>
    /// Gets or sets whether the connection is currently active.
    /// </summary>
    public bool IsConnected { get; set; }

    /// <summary>
    /// Gets or sets the current connection status.
    /// </summary>
    public ConnectionState Status { get; set; }

    /// <summary>
    /// Gets or sets the last time a successful connection was established.
    /// </summary>
    public DateTime? LastConnected { get; set; }

    /// <summary>
    /// Gets or sets the last time the connection was lost.
    /// </summary>
    public DateTime? LastDisconnected { get; set; }

    /// <summary>
    /// Gets or sets the error message if any.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets additional connection details.
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new();
}

/// <summary>
/// Represents the state of a connection.
/// </summary>
public enum ConnectionState
{
    /// <summary>
    /// The connection is disconnected.
    /// </summary>
    Disconnected,

    /// <summary>
    /// The connection is connecting.
    /// </summary>
    Connecting,

    /// <summary>
    /// The connection is connected.
    /// </summary>
    Connected,

    /// <summary>
    /// The connection is reconnecting.
    /// </summary>
    Reconnecting,

    /// <summary>
    /// The connection failed.
    /// </summary>
    Failed,

    /// <summary>
    /// The connection state is unknown.
    /// </summary>
    Unknown
}
