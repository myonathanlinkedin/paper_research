namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the state of a connection.
/// </summary>
public enum ConnectionState
{
    /// <summary>
    /// Disconnected state.
    /// </summary>
    Disconnected,

    /// <summary>
    /// Connecting state.
    /// </summary>
    Connecting,

    /// <summary>
    /// Connected state.
    /// </summary>
    Connected,

    /// <summary>
    /// Disconnecting state.
    /// </summary>
    Disconnecting,

    /// <summary>
    /// Failed state.
    /// </summary>
    Failed,

    /// <summary>
    /// Unknown state.
    /// </summary>
    Unknown
} 