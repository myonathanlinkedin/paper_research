using System;

namespace CodeSage.Core.Models.Metrics
{
    public class ResourceUsage
    {
        public double CpuUsage { get; set; }
        public long MemoryUsage { get; set; }
        public int ThreadCount { get; set; }
        public int HandleCount { get; set; }
        public DateTime Timestamp { get; set; }
        public string ProcessName { get; set; }
        public int ProcessId { get; set; }
        public Dictionary<string, double> CustomMetrics { get; set; }
    }
} 