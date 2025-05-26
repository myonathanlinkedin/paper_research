using System;

namespace CodeSage.Core.Health.Models
{
    public class ServiceHealthStatus
    {
        public string Endpoint { get; set; } = string.Empty;
        public bool IsHealthy { get; set; }
        public DateTime LastCheck { get; set; }
        public int ConsecutiveFailures { get; set; }
        public int ConsecutiveSuccesses { get; set; }
    }
} 