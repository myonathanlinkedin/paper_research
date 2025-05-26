using System;
using System.Collections.Generic;

namespace CodeSage.Core.Models.Common
{
    public class HealthStatus
    {
        public bool IsHealthy { get; set; }
        public Dictionary<string, object> Details { get; set; } = new();
        public string? StatusMessage { get; set; }
        public DateTime LastCheckTime { get; set; } = DateTime.UtcNow;
        public List<string> Warnings { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public Dictionary<string, object> Metrics { get; set; } = new();
    }
} 