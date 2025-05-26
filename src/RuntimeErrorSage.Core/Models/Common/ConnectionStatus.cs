using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Common
{
    /// <summary>
    /// Represents the connection status of a client or service.
    /// </summary>
    public class ConnectionStatus
    {
        /// <summary>
        /// Gets or sets whether the connection is active.
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// Gets or sets the last time the connection was established.
        /// </summary>
        public DateTime? LastConnected { get; set; }

        /// <summary>
        /// Gets or sets the last time the connection was disconnected.
        /// </summary>
        public DateTime? LastDisconnected { get; set; }

        /// <summary>
        /// Gets or sets the current status of the connection.
        /// </summary>
        public ConnectionState Status { get; set; }

        /// <summary>
        /// Gets or sets the error message if the connection failed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets additional details about the connection status.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();
    }

    /// <summary>
    /// Defines the possible states of a connection.
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        /// The connection is disconnected.
        /// </summary>
        Disconnected,

        /// <summary>
        /// The connection is in the process of connecting.
        /// </summary>
        Connecting,

        /// <summary>
        /// The connection is connected and active.
        /// </summary>
        Connected,

        /// <summary>
        /// The connection is in the process of disconnecting.
        /// </summary>
        Disconnecting,

        /// <summary>
        /// The connection has failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The connection is in a reconnecting state.
        /// </summary>
        Reconnecting,

        /// <summary>
        /// The connection status is unknown.
        /// </summary>
        Unknown
    }
} 