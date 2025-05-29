using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Options;

/// <summary>
/// Configuration options for Redis pattern storage.
/// </summary>
public class RedisPatternStorageOptions
{
    /// <summary>
    /// Gets or sets the key prefix for Redis keys.
    /// </summary>
    public string KeyPrefix { get; } = "pattern:";

    /// <summary>
    /// Gets or sets the connection string for Redis.
    /// </summary>
    public string ConnectionString { get; } = "localhost:6379";

    /// <summary>
    /// Gets or sets the database number to use.
    /// </summary>
    public int Database { get; } = 0;

    /// <summary>
    /// Gets or sets the timeout in milliseconds.
    /// </summary>
    public int TimeoutMs { get; } = 5000;
} 






