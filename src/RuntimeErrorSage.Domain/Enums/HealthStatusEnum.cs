namespace RuntimeErrorSage.Domain.Enums;

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

/// <summary>
/// Extension methods for HealthStatusEnum.
/// </summary>
public static class HealthStatusEnumExtensions
{
    /// <summary>
    /// Converts a HealthStatusEnum to Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.
    /// </summary>
    /// <param name="status">The health status enum.</param>
    /// <returns>The corresponding Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.</returns>
    public static HealthStatus ToHealthStatus(this HealthStatusEnum status)
    {
        return status switch
        {
            HealthStatusEnum.Healthy => HealthStatus.Healthy,
            HealthStatusEnum.Degraded => HealthStatus.Degraded,
            HealthStatusEnum.Unhealthy => HealthStatus.Unhealthy,
            HealthStatusEnum.Critical => HealthStatus.Unhealthy,
            _ => HealthStatus.Unhealthy
        };
    }

    /// <summary>
    /// Converts a Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus to HealthStatusEnum.
    /// </summary>
    /// <param name="status">The health status.</param>
    /// <returns>The corresponding HealthStatusEnum.</returns>
    public static HealthStatusEnum ToHealthStatusEnum(this HealthStatus status)
    {
        return status switch
        {
            HealthStatus.Healthy => HealthStatusEnum.Healthy,
            HealthStatus.Degraded => HealthStatusEnum.Degraded,
            HealthStatus.Unhealthy => HealthStatusEnum.Unhealthy,
            _ => HealthStatusEnum.Unknown
        };
    }
} 
