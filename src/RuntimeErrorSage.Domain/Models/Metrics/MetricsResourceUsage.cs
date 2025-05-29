using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Metrics
{
    /// <summary>
    /// Represents resource usage metrics for a specific phase or operation
    /// </summary>
    public class MetricsResourceUsage
    {
        /// <summary>
        /// Memory usage in megabytes
        /// </summary>
        public double MemoryUsage { get; }

        /// <summary>
        /// CPU usage as a percentage
        /// </summary>
        public double CpuUsage { get; }

        /// <summary>
        /// Timestamp when the metrics were collected
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the disk usage in bytes.
        /// </summary>
        public long DiskUsage { get; }

        /// <summary>
        /// Gets or sets the network usage in bytes.
        /// </summary>
        public long NetworkUsage { get; }

        /// <summary>
        /// Gets or sets the number of threads.
        /// </summary>
        public int ThreadCount { get; }

        /// <summary>
        /// Gets or sets the number of handles.
        /// </summary>
        public int HandleCount { get; }

        /// <summary>
        /// Gets or sets additional metadata.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new();

        /// <summary>
        /// Creates a new instance of ResourceUsage with the current system metrics.
        /// </summary>
        public static MetricsResourceUsage GetCurrent()
        {
            return new MetricsResourceUsage
            {
                CpuUsage = GetCpuUsage(),
                MemoryUsage = GetMemoryUsage(),
                DiskUsage = GetDiskUsage(),
                NetworkUsage = GetNetworkUsage(),
                ThreadCount = GetThreadCount(),
                HandleCount = GetHandleCount(),
                Timestamp = DateTime.UtcNow
            };
        }

        private static double GetCpuUsage()
        {
            // TODO: Implement actual CPU usage measurement
            return 0;
        }

        private static long GetMemoryUsage()
        {
            // TODO: Implement actual memory usage measurement
            return 0;
        }

        private static long GetDiskUsage()
        {
            // TODO: Implement actual disk usage measurement
            return 0;
        }

        private static long GetNetworkUsage()
        {
            // TODO: Implement actual network usage measurement
            return 0;
        }

        private static int GetThreadCount()
        {
            // TODO: Implement actual thread count measurement
            return 0;
        }

        private static int GetHandleCount()
        {
            // TODO: Implement actual handle count measurement
            return 0;
        }
    }
} 







