using System.Threading.Tasks;
using RuntimeErrorSage.Model.Models.Metrics;
using RuntimeErrorSage.Model.Models.Monitoring;

namespace RuntimeErrorSage.Model.Interfaces
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

        Task<MonitoringResult> MonitorAsync(string applicationId);
    }
} 

