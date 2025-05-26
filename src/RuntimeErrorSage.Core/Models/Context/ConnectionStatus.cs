using System;

namespace RuntimeErrorSage.Core.Models.Context
{
    /// <summary>
    /// Represents the status of a connection.
    /// </summary>
    public class ConnectionStatus
    {
        /// <summary>
        /// Gets or sets the unique identifier for the connection status.
        /// </summary>
        public string StatusId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the timestamp when the status was recorded.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the name of the connection.
        /// </summary>
        public string ConnectionName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the connection.
        /// </summary>
        public ConnectionType ConnectionType { get; set; }

        /// <summary>
        /// Gets or sets the current status of the connection.
        /// </summary>
        public ConnectionState State { get; set; }

        /// <summary>
        /// Gets or sets the latency of the connection in milliseconds.
        /// </summary>
        public double LatencyMs { get; set; }

        /// <summary>
        /// Gets or sets the error rate of the connection (0-1).
        /// </summary>
        public double ErrorRate { get; set; }

        /// <summary>
        /// Gets or sets the number of active connections.
        /// </summary>
        public int ActiveConnections { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of allowed connections.
        /// </summary>
        public int MaxConnections { get; set; }

        /// <summary>
        /// Gets or sets the connection timeout in milliseconds.
        /// </summary>
        public int TimeoutMs { get; set; }

        /// <summary>
        /// Gets or sets the last error message if any.
        /// </summary>
        public string? LastErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the time of the last successful connection.
        /// </summary>
        public DateTime? LastSuccessfulConnection { get; set; }

        /// <summary>
        /// Gets or sets the time of the last failed connection.
        /// </summary>
        public DateTime? LastFailedConnection { get; set; }

        /// <summary>
        /// Gets or sets any additional metadata about the connection.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the connection is healthy.
        /// </summary>
        public bool IsHealthy => State == ConnectionState.Connected && ErrorRate < 0.1 && LatencyMs < TimeoutMs;

        /// <summary>
        /// Gets or sets the connection utilization (0-1).
        /// </summary>
        public double Utilization => MaxConnections > 0 ? (double)ActiveConnections / MaxConnections : 0;
    }

    /// <summary>
    /// Represents the type of a connection.
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// A database connection.
        /// </summary>
        Database,

        /// <summary>
        /// An HTTP connection.
        /// </summary>
        Http,

        /// <summary>
        /// A message queue connection.
        /// </summary>
        MessageQueue,

        /// <summary>
        /// A file system connection.
        /// </summary>
        FileSystem,

        /// <summary>
        /// A cache connection.
        /// </summary>
        Cache,

        /// <summary>
        /// A custom connection type.
        /// </summary>
        Custom
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
        /// The connection is disconnecting.
        /// </summary>
        Disconnecting,

        /// <summary>
        /// The connection has failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The connection is in an unknown state.
        /// </summary>
        Unknown
    }
} 