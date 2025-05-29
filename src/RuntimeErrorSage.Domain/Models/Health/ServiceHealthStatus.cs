using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Health.Models
{
    public class ServiceHealthStatus
    {
        public string Endpoint { get; } = string.Empty;
        public bool IsHealthy { get; }
        public DateTime LastCheck { get; }
        public int ConsecutiveFailures { get; }
        public int ConsecutiveSuccesses { get; }
    }
} 







