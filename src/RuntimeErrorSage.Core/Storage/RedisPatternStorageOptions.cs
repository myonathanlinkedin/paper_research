using System;

namespace RuntimeErrorSage.Core.Storage
{
    public class RedisPatternStorageOptions
    {
        public string ConnectionString { get; set; }
        public string KeyPrefix { get; set; } = "pattern:";
        public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromHours(24);
        public int RetryCount { get; set; } = 3;
        public TimeSpan RetryInterval { get; set; } = TimeSpan.FromSeconds(1);
        public bool UseCompression { get; set; } = true;
        public int CompressionLevel { get; set; } = 6;
        public bool EnableMetrics { get; set; } = true;
        public bool EnableHealthChecks { get; set; } = true;
        public TimeSpan HealthCheckInterval { get; set; } = TimeSpan.FromMinutes(5);
        public TimeSpan HealthCheckTimeout { get; set; } = TimeSpan.FromSeconds(10);
        public bool EnableLogging { get; set; } = true;
        public string LogLevel { get; set; } = "Information";
    }
} 
