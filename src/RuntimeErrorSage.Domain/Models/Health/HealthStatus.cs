using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Application.Models.Health
{
    /// <summary>
    /// Represents the health status of a service or component
    /// </summary>
    public enum HealthStatus
    {
        /// <summary>
        /// Service is healthy and functioning normally
        /// </summary>
        Healthy = 0,

        /// <summary>
        /// Service is degraded but still operational
        /// </summary>
        Degraded = 1,

        /// <summary>
        /// Service is unhealthy and not functioning properly
        /// </summary>
        Unhealthy = 2,

        /// <summary>
        /// Service status is unknown
        /// </summary>
        Unknown = 3,

        /// <summary>
        /// Service is in maintenance mode
        /// </summary>
        Maintenance = 4,

        /// <summary>
        /// Service is starting up
        /// </summary>
        Starting = 5,

        /// <summary>
        /// Service is shutting down
        /// </summary>
        Stopping = 6,

        /// <summary>
        /// Service is stopped
        /// </summary>
        Stopped = 7
    }
} 






