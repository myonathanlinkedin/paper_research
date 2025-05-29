using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Application.Health.Interfaces
{
    public interface IHealthCheckProvider
    {
        string Name { get; }
        Task<Dictionary<string, double>> GetMetricsAsync();
    }
} 







