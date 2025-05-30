using System;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.MCP
{
    /// <summary>
    /// Represents the status of an MCP (Model Control Protocol) connection.
    /// </summary>
    public class MCPConnectionStatus
    {
        /// <summary>
        /// Gets or sets the connection state.
        /// </summary>
        public ConnectionState State { get; set; } = ConnectionState.Disconnected;

        /// <summary>
        /// Gets or sets the last connection attempt timestamp.
        /// </summary>
        public DateTime LastAttempt { get; set; }

        /// <summary>
        /// Gets or sets the last successful connection timestamp.
        /// </summary>
        public DateTime LastSuccess { get; set; }

        /// <summary>
        /// Gets or sets the last connection error message.
        /// </summary>
        public string LastError { get; set; }

        /// <summary>
        /// Gets or sets the last connected timestamp.
        /// </summary>
        public DateTime LastConnected { get; set; }
    }
} 