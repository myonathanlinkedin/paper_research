using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Model.Models.Metrics
{
    /// <summary>
    /// Represents metrics for a component.
    /// </summary>
    public class ComponentMetrics
    {
        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; set; }
        
        /// <summary>
        /// Gets or sets the timestamp when the metrics were collected.
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// Gets or sets the CPU usage percentage.
        /// </summary>
        public double CpuUsagePercent { get; set; }
        
        /// <summary>
        /// Gets or sets the memory usage in bytes.
        /// </summary>
        public long MemoryUsageBytes { get; set; }
        
        /// <summary>
        /// Gets or sets the number of requests processed.
        /// </summary>
        public long RequestCount { get; set; }
        
        /// <summary>
        /// Gets or sets the average request duration in milliseconds.
        /// </summary>
        public double AverageRequestDurationMs { get; set; }
        
        /// <summary>
        /// Gets or sets the error count.
        /// </summary>
        public long ErrorCount { get; set; }
        
        /// <summary>
        /// Gets or sets the error rate as a percentage.
        /// </summary>
        public double ErrorRatePercent { get; set; }
        
        /// <summary>
        /// Gets or sets additional custom metrics.
        /// </summary>
        public Dictionary<string, object> CustomMetrics { get; set; } = new();
    }
} 
