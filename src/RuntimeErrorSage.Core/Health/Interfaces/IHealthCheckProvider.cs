using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Model.Health.Interfaces
{
    public interface IHealthCheckProvider
    {
        string Name { get; }
        Task<Dictionary<string, double>> GetMetricsAsync();
    }
} 

