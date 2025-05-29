using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Storage
{
    public class RedisPatternStorageOptions
    {
        public string ConnectionString { get; }
        public string KeyPrefix { get; } = "pattern:";
        public TimeSpan DefaultExpiration { get; } = TimeSpan.FromHours(24);
        public int RetryCount { get; } = 3;
        public TimeSpan RetryInterval { get; } = TimeSpan.FromSeconds(1);
        public bool UseCompression { get; } = true;
        public int CompressionLevel { get; } = 6;
        public bool EnableMetrics { get; } = true;
        public bool EnableHealthChecks { get; } = true;
        public TimeSpan HealthCheckInterval { get; } = TimeSpan.FromMinutes(5);
        public TimeSpan HealthCheckTimeout { get; } = TimeSpan.FromSeconds(10);
        public bool EnableLogging { get; } = true;
        public string LogLevel { get; } = "Information";
    }
} 





