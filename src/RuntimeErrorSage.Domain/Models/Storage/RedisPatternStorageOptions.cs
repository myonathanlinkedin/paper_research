using System;

namespace RuntimeErrorSage.Domain.Models.Storage;

/// <summary>
/// Configuration options for Redis pattern storage
/// </summary>
public class RedisPatternStorageOptions
{
    /// <summary>
    /// Gets or sets the connection string for Redis
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the instance name for Redis
    /// </summary>
    public string InstanceName { get; set; }

    /// <summary>
    /// Gets or sets the database number to use
    /// </summary>
    public int Database { get; set; }

    /// <summary>
    /// Gets or sets the key prefix for all stored patterns
    /// </summary>
    public string KeyPrefix { get; set; }

    /// <summary>
    /// Gets or sets the default expiration time for cached patterns
    /// </summary>
    public TimeSpan DefaultExpiration { get; set; }

    /// <summary>
    /// Gets or sets whether to use SSL for Redis connection
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// Gets or sets the connection timeout in milliseconds
    /// </summary>
    public int ConnectionTimeout { get; set; }

    /// <summary>
    /// Gets or sets the sync timeout in milliseconds
    /// </summary>
    public int SyncTimeout { get; set; }

    /// <summary>
    /// Gets or sets whether to abort on connection failure
    /// </summary>
    public bool AbortOnConnectFail { get; set; }

    /// <summary>
    /// Gets or sets the retry count for failed operations
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    /// Gets or sets the retry interval in milliseconds
    /// </summary>
    public int RetryInterval { get; set; }

    /// <summary>
    /// Initializes a new instance of the RedisPatternStorageOptions class
    /// </summary>
    public RedisPatternStorageOptions()
    {
        ConnectionString = "localhost:6379";
        InstanceName = "RuntimeErrorSage";
        Database = 0;
        KeyPrefix = "pattern:";
        DefaultExpiration = TimeSpan.FromHours(24);
        UseSsl = false;
        ConnectionTimeout = 5000;
        SyncTimeout = 5000;
        AbortOnConnectFail = false;
        RetryCount = 3;
        RetryInterval = 1000;
    }
} 
