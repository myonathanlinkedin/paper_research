namespace RuntimeErrorSage.Core.Storage.Models;

/// <summary>
/// Configuration options for Redis pattern storage
/// </summary>
public class RedisPatternStorageOptions
{
    /// <summary>
    /// Redis connection string
    /// </summary>
    public string ConnectionString { get; set; } = "localhost:6379";

    /// <summary>
    /// Key prefix for pattern storage
    /// </summary>
    public string KeyPrefix { get; set; } = "RuntimeErrorSage:patterns:";

    /// <summary>
    /// Pattern retention period in days
    /// </summary>
    public int PatternRetentionPeriod { get; set; } = 90;

    /// <summary>
    /// Database number to use
    /// </summary>
    public int Database { get; set; } = 0;

    /// <summary>
    /// Connection timeout in seconds
    /// </summary>
    public int ConnectionTimeout { get; set; } = 30;

    /// <summary>
    /// Operation timeout in seconds
    /// </summary>
    public int OperationTimeout { get; set; } = 10;
} 