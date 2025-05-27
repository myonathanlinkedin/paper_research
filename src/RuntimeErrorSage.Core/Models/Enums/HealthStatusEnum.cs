namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the health status of a system component.
/// </summary>
public enum HealthStatusEnum
{
    /// <summary>
    /// The component is healthy.
    /// </summary>
    Healthy = 0,

    /// <summary>
    /// The component is degraded but still functional.
    /// </summary>
    Degraded = 1,

    /// <summary>
    /// The component is unhealthy and not functioning properly.
    /// </summary>
    Unhealthy = 2,

    /// <summary>
    /// The component is in a critical state.
    /// </summary>
    Critical = 3,

    /// <summary>
    /// The component's health status is unknown.
    /// </summary>
    Unknown = 4
} 