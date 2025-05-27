using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Services.Models;

namespace RuntimeErrorSage.Core.Services.Interfaces
{
    /// <summary>
    /// Service for collecting and calculating metrics.
    /// </summary>
    public interface IMetricsCollector
    {
        /// <summary>
        /// Calculates the health score for a component.
        /// </summary>
        /// <param name="node">The dependency node representing the component</param>
        /// <returns>The component health score (0.0 to 1.0)</returns>
        Task<double> CalculateComponentHealthAsync(DependencyNode node);

        /// <summary>
        /// Calculates the reliability score for a component.
        /// </summary>
        /// <param name="node">The dependency node representing the component</param>
        /// <returns>The component reliability score (0.0 to 1.0)</returns>
        Task<double> CalculateReliabilityAsync(DependencyNode node);

        /// <summary>
        /// Collects metrics for the current phase.
        /// </summary>
        /// <returns>A dictionary of metric names and values</returns>
        Task<Dictionary<string, MetricValue>> CollectMetricsAsync();
    }
} 