namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the health status of a component.
/// </summary>
public enum HealthStatus
{
    /// <summary>
    /// Component is healthy and functioning normally.
    /// </summary>
    Healthy,

    /// <summary>
    /// Component is degraded but still functioning.
    /// </summary>
    Degraded,

    /// <summary>
    /// Component is unhealthy and not functioning properly.
    /// </summary>
    Unhealthy,

    /// <summary>
    /// Component health status is unknown.
    /// </summary>
    Unknown
} 