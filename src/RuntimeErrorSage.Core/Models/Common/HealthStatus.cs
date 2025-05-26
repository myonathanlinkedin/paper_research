using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Common
{
    /// <summary>
    /// Represents detailed health status information.
    /// </summary>
    public class HealthStatusInfo
    {
        public bool IsHealthy { get; set; }
        public Dictionary<string, object> Details { get; set; } = new();
        public string? StatusMessage { get; set; }
        public DateTime LastCheckTime { get; set; } = DateTime.UtcNow;
        public List<string> Warnings { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public Dictionary<string, object> Metrics { get; set; } = new();
    }

    /// <summary>
    /// Represents the health status of a component.
    /// </summary>
    public enum HealthStatus
    {
        /// <summary>
        /// The component is healthy.
        /// </summary>
        Healthy,

        /// <summary>
        /// The component is degraded but still functioning.
        /// </summary>
        Degraded,

        /// <summary>
        /// The component is unhealthy.
        /// </summary>
        Unhealthy,

        /// <summary>
        /// The component's health status is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The component is starting up.
        /// </summary>
        Starting,

        /// <summary>
        /// The component is shutting down.
        /// </summary>
        ShuttingDown,

        /// <summary>
        /// The component is in maintenance mode.
        /// </summary>
        Maintenance,

        /// <summary>
        /// The component requires attention.
        /// </summary>
        Warning
    }
} 
