using System;

namespace RuntimeErrorSage.Model.Options;

/// <summary>
/// Configuration options for Redis pattern storage.
/// </summary>
public class RedisPatternStorageOptions
{
    /// <summary>
    /// Gets or sets the key prefix for Redis keys.
    /// </summary>
    public string KeyPrefix { get; set; } = "pattern:";

    /// <summary>
    /// Gets or sets the connection string for Redis.
    /// </summary>
    public string ConnectionString { get; set; } = "localhost:6379";

    /// <summary>
    /// Gets or sets the database number to use.
    /// </summary>
    public int Database { get; set; } = 0;

    /// <summary>
    /// Gets or sets the timeout in milliseconds.
    /// </summary>
    public int TimeoutMs { get; set; } = 5000;
} 
