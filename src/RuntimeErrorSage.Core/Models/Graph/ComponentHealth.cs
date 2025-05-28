using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Represents the health status of a component in the dependency graph.
    /// </summary>
    public class ComponentHealth
    {
        /// <summary>
        /// Gets or sets the component id.
        /// </summary>
        public string ComponentId { get; set; }

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
        /// Gets or sets the status message.
        /// </summary>
        public string StatusMessage { get; set; }
        
        /// <summary>
        /// Gets or sets the health issues.
        /// </summary>
        public List<string> Issues { get; set; } = new();
        
        /// <summary>
        /// Gets or sets additional metrics.
        /// </summary>
        public Dictionary<string, object> Metrics { get; set; } = new();
    }
} 
