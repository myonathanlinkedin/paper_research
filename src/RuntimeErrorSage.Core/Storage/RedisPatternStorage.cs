using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;
using RuntimeErrorSage.Core.Interfaces.Storage;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Storage.Models;
using RuntimeErrorSage.Core.Storage.Exceptions;

namespace RuntimeErrorSage.Core.Storage;

/// <summary>
/// Redis-based implementation of pattern storage
/// </summary>
public class RedisPatternStorage : IPatternStorage, IDisposable
{
    private readonly ILogger<RedisPatternStorage> _logger;
    private readonly RedisPatternStorageOptions _options;
    private readonly ConnectionMultiplexer _redis;
    private bool _disposed;

    public RedisPatternStorage(
        ILogger<RedisPatternStorage> logger,
        IOptions<RedisPatternStorageOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

        try
        {
            var config = new ConfigurationOptions
            {
                EndPoints = { _options.ConnectionString },
                ConnectTimeout = _options.ConnectionTimeout * 1000,
                SyncTimeout = _options.OperationTimeout * 1000,
                DefaultDatabase = _options.Database
            };

            _redis = ConnectionMultiplexer.Connect(config);
            _logger.LogInformation("Connected to Redis at {ConnectionString}", _options.ConnectionString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to Redis at {ConnectionString}", _options.ConnectionString);
            throw new PatternStorageException("Failed to connect to Redis", ex);
        }
    }

    public async Task StorePatternAsync(string patternId, string pattern)
    {
        try
        {
            var db = _redis.GetDatabase();
            var key = $"{_options.KeyPrefix}{patternId}";
            await db.StringSetAsync(key, pattern, TimeSpan.FromDays(_options.PatternRetentionPeriod));
            _logger.LogDebug("Stored pattern {PatternId}", patternId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to store pattern {PatternId}", patternId);
            throw new PatternStorageException($"Failed to store pattern {patternId}", ex);
        }
    }

    public async Task<string?> GetPatternAsync(string patternId)
    {
        try
        {
            var db = _redis.GetDatabase();
            var key = $"{_options.KeyPrefix}{patternId}";
            var pattern = await db.StringGetAsync(key);
            return pattern.HasValue ? pattern.ToString() : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get pattern {PatternId}", patternId);
            throw new PatternStorageException($"Failed to get pattern {patternId}", ex);
        }
    }

    public async Task DeletePatternAsync(string patternId)
    {
        try
        {
            var db = _redis.GetDatabase();
            var key = $"{_options.KeyPrefix}{patternId}";
            await db.KeyDeleteAsync(key);
            _logger.LogDebug("Deleted pattern {PatternId}", patternId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete pattern {PatternId}", patternId);
            throw new PatternStorageException($"Failed to delete pattern {patternId}", ex);
        }
    }

    public async Task<IEnumerable<string>> GetAllPatternsAsync()
    {
        try
        {
            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_options.ConnectionString);
            var keys = server.Keys(pattern: $"{_options.KeyPrefix}*");
            var patterns = new List<string>();

            foreach (var key in keys)
            {
                var pattern = await db.StringGetAsync(key);
                if (pattern.HasValue)
                {
                    patterns.Add(pattern.ToString());
                }
            }

            return patterns;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all patterns");
            throw new PatternStorageException("Failed to get all patterns", ex);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _redis?.Dispose();
            }
            _disposed = true;
        }
    }
}

/// <summary>
/// Configuration options for Redis pattern storage.
/// </summary>
public class RedisPatternStorageOptions
{
    /// <summary>
    /// Gets or sets the Redis connection string.
    /// </summary>
    public string ConnectionString { get; set; } = "localhost:6379";

    /// <summary>
    /// Gets or sets the key prefix for Redis keys.
    /// </summary>
    public string KeyPrefix { get; set; } = "RuntimeErrorSage:patterns:";

    /// <summary>
    /// Gets or sets the pattern retention period.
    /// </summary>
    public TimeSpan PatternRetentionPeriod { get; set; } = TimeSpan.FromDays(90);

    /// <summary>
    /// Gets or sets the connection timeout.
    /// </summary>
    public int ConnectionTimeout { get; set; } = 5000;

    /// <summary>
    /// Gets or sets the operation timeout.
    /// </summary>
    public int OperationTimeout { get; set; } = 5000;

    /// <summary>
    /// Gets or sets the database.
    /// </summary>
    public int Database { get; set; } = 0;
}

/// <summary>
/// Exception thrown when pattern storage operations fail.
/// </summary>
public class PatternStorageException : Exception
{
    public PatternStorageException() { }
    public PatternStorageException(string message) : base(message) { }
    public PatternStorageException(string message, Exception inner) : base(message, inner) { }
} 