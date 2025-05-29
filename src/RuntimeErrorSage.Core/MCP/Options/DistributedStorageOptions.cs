using System;

namespace RuntimeErrorSage.Model.MCP.Options;

public class DistributedStorageOptions
{
    public string ConnectionString { get; set; } = "localhost:6379";
    public string KeyPrefix { get; set; } = "RuntimeErrorSage:mcp:";
    public TimeSpan ContextRetentionPeriod { get; set; } = TimeSpan.FromDays(30);
    public TimeSpan PatternRetentionPeriod { get; set; } = TimeSpan.FromDays(90);
    public bool EnableDataPartitioning { get; set; } = true;
    public int PartitionCount { get; set; } = 4;
    public bool EnableBackup { get; set; } = true;
    public TimeSpan BackupInterval { get; set; } = TimeSpan.FromHours(1);
    public string BackupPath { get; set; } = "backups/redis";
    public int MaxBackupCount { get; set; } = 24;
    public bool EnableDistributedCache { get; set; } = true;
    public TimeSpan CacheExpiration { get; set; } = TimeSpan.FromMinutes(5);
    public int CacheMaxSize { get; set; } = 10000;
} 
