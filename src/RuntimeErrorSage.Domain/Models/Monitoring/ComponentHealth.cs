using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Monitoring
{
    /// <summary>
    /// Represents component health status.
    /// </summary>
    public class ComponentHealth
    {
        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; set; }
        
        /// <summary>
        /// Gets or sets whether the component is healthy.
        /// </summary>
        public bool IsHealthy { get; set; }
        
        /// <summary>
        /// Gets or sets the health score (0-100).
        /// </summary>
        public int HealthScore { get; set; }
        
        /// <summary>
        /// Gets or sets any health issues detected.
        /// </summary>
        public List<string> Issues { get; set; } = new();
    }
} 