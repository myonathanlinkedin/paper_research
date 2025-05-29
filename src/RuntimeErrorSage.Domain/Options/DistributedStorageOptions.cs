using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.MCP.Options;

public class DistributedStorageOptions
{
    public string ConnectionString { get; } = "localhost:6379";
    public string KeyPrefix { get; } = "RuntimeErrorSage:mcp:";
    public TimeSpan ContextRetentionPeriod { get; } = TimeSpan.FromDays(30);
    public TimeSpan PatternRetentionPeriod { get; } = TimeSpan.FromDays(90);
    public bool EnableDataPartitioning { get; } = true;
    public int PartitionCount { get; } = 4;
    public bool EnableBackup { get; } = true;
    public TimeSpan BackupInterval { get; } = TimeSpan.FromHours(1);
    public string BackupPath { get; } = "backups/redis";
    public int MaxBackupCount { get; } = 24;
    public bool EnableDistributedCache { get; } = true;
    public TimeSpan CacheExpiration { get; } = TimeSpan.FromMinutes(5);
    public int CacheMaxSize { get; } = 10000;
} 






