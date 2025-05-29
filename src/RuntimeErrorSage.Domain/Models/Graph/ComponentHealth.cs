using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Graph
{
    /// <summary>
    /// Represents the health status of a component in the dependency graph.
    /// </summary>
    public class ComponentHealth
    {
        /// <summary>
        /// Gets or sets the component id.
        /// </summary>
        public string ComponentId { get; }

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; }
        
        /// <summary>
        /// Gets or sets whether the component is healthy.
        /// </summary>
        public bool IsHealthy { get; }
        
        /// <summary>
        /// Gets or sets the health score (0-100).
        /// </summary>
        public int HealthScore { get; }
        
        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        public string StatusMessage { get; }
        
        /// <summary>
        /// Gets or sets the health issues.
        /// </summary>
        public IReadOnlyCollection<Issues> Issues { get; } = new();
        
        /// <summary>
        /// Gets or sets additional metrics.
        /// </summary>
        public Dictionary<string, object> Metrics { get; set; } = new();
    }
} 






