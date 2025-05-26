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

namespace RuntimeErrorSage.Core.Storage;

/// <summary>
/// Redis implementation of pattern storage.
/// </summary>
public class RedisPatternStorage : IPatternStorage, IDisposable
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<RedisPatternStorage> _logger;
    private readonly RedisPatternStorageOptions _options;
    private readonly SemaphoreSlim _semaphore;
    private bool _disposed;

    public RedisPatternStorage(
        IConnectionMultiplexer redis,
        ILogger<RedisPatternStorage> logger,
        IOptions<RedisPatternStorageOptions> options)
    {
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _semaphore = new SemaphoreSlim(1, 1);
    }

    public async Task<List<ErrorPattern>> GetPatternsAsync()
    {
        ThrowIfDisposed();

        try
        {
            await _semaphore.WaitAsync();
            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var patterns = new List<ErrorPattern>();

            await foreach (var key in server.KeysAsync(pattern: $"{_options.KeyPrefix}pattern:*"))
            {
                var value = await db.StringGetAsync(key);
                if (value.HasValue)
                {
                    var pattern = JsonSerializer.Deserialize<ErrorPattern>(value);
                    if (pattern != null)
                    {
                        patterns.Add(pattern);
                    }
                }
            }

            return patterns;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving error patterns");
            throw new PatternStorageException("Failed to retrieve patterns", ex);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task SavePatternsAsync(List<ErrorPattern> patterns)
    {
        ArgumentNullException.ThrowIfNull(patterns);
        ThrowIfDisposed();

        try
        {
            await _semaphore.WaitAsync();
            var db = _redis.GetDatabase();
            var batch = db.CreateBatch();

            foreach (var pattern in patterns)
            {
                var key = $"{_options.KeyPrefix}pattern:{pattern.Id}";
                var value = JsonSerializer.SerializeToUtf8Bytes(pattern);
                await batch.StringSetAsync(key, value, _options.PatternRetentionPeriod);
            }

            batch.Execute();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving error patterns");
            throw new PatternStorageException("Failed to save patterns", ex);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<ConnectionStatus> GetConnectionStatusAsync()
    {
        ThrowIfDisposed();

        try
        {
            var status = new ConnectionStatus
            {
                IsConnected = _redis.IsConnected,
                Status = _redis.IsConnected ? ConnectionState.Connected : ConnectionState.Disconnected,
                LastConnected = _redis.IsConnected ? DateTime.UtcNow : null,
                Details = new Dictionary<string, object>
                {
                    ["Endpoint"] = _redis.GetEndPoints().First().ToString(),
                    ["ClientName"] = _redis.ClientName
                }
            };

            return status;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting connection status");
            return new ConnectionStatus
            {
                IsConnected = false,
                Status = ConnectionState.Failed,
                LastDisconnected = DateTime.UtcNow,
                ErrorMessage = ex.Message
            };
        }
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(RedisPatternStorage));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _semaphore.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
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