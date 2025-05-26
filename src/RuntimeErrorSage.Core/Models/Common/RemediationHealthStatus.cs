using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Common
{
    public class RemediationHealthStatus
    {
        public bool IsHealthy { get; set; }
        public Dictionary<string, object> Details { get; set; } = new();
        public string? StatusMessage { get; set; }
        public DateTime LastCheckTime { get; set; }
        public List<string> Warnings { get; set; } = new();
        public List<string> Errors { get; set; } = new();
    }
} 
