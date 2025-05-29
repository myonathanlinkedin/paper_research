using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Health
{
    /// <summary>
    /// Represents the health status of the system.
    /// </summary>
    public class SystemHealthStatus
    {
        /// <summary>
        /// Gets or sets whether the system is healthy.
        /// </summary>
        public bool IsHealthy { get; set; }

        /// <summary>
        /// Gets or sets additional health status details.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();
    }
} 
