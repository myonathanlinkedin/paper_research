using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Interface for monitoring service operations.
    /// </summary>
    public interface IMonitoringService
    {
        /// <summary>
        /// Starts monitoring a component.
        /// </summary>
        /// <param name="componentName">The component name.</param>
        /// <returns>The operation result.</returns>
        Task<MonitoringResult> StartMonitoringAsync(string componentName);
        
        /// <summary>
        /// Stops monitoring a component.
        /// </summary>
        /// <param name="componentName">The component name.</param>
        /// <returns>The operation result.</returns>
        Task<MonitoringResult> StopMonitoringAsync(string componentName);
        
        /// <summary>
        /// Gets the current metrics for a component.
        /// </summary>
        /// <param name="componentName">The component name.</param>
        /// <returns>The component metrics.</returns>
        Task<ComponentMetrics> GetMetricsAsync(string componentName);
        
        /// <summary>
        /// Records a custom metric.
        /// </summary>
        /// <param name="componentName">The component name.</param>
        /// <param name="metricName">The metric name.</param>
        /// <param name="value">The metric value.</param>
        /// <returns>The operation result.</returns>
        Task<MonitoringResult> RecordMetricAsync(string componentName, string metricName, double value);
        
        /// <summary>
        /// Gets the health status of a component.
        /// </summary>
        /// <param name="componentName">The component name.</param>
        /// <returns>The component health status.</returns>
        Task<ComponentHealth> GetHealthStatusAsync(string componentName);
    }
    
    /// <summary>
    /// Represents the result of a monitoring operation.
    /// </summary>
    public class MonitoringResult
    {
        /// <summary>
        /// Gets or sets whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Gets or sets the error message if the operation failed.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
    
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
