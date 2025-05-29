using System.Threading.Tasks;

namespace RuntimeErrorSage.Model.Health.Interfaces
{
    /// <summary>
    /// Defines the interface for health check providers.
    /// </summary>
    public interface ICustomHealthCheck
    {
        /// <summary>
        /// Gets the name of the health check.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the current health status.
        /// </summary>
        Task<bool> CheckHealthAsync();
    }
} 

