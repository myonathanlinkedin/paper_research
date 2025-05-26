using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Defines the interface for health check providers.
    /// </summary>
    public interface ICustomHealthCheck
    {
        /// <summary>
        /// Gets the current health status.
        /// </summary>
        Task<bool> CheckHealthAsync();
    }
} 
