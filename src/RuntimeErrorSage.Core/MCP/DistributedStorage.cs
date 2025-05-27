using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.MCP.Options;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Storage;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Text.Json;
using RuntimeErrorSage.Core.MCP.Exceptions;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.MCP;

public class RedisDistributedStorage : IDistributedStorage, IDisposable
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<RedisDistributedStorage> _logger;
    private readonly DistributedStorageOptions _options;
    private readonly SemaphoreSlim _semaphore;
    private readonly ConcurrentDictionary<string, byte[]> _cache;
    private readonly System.Timers.Timer _backupTimer;
    private readonly SemaphoreSlim _backupLock;
    private bool _disposed;
    private long _cacheHits;
    private long _cacheMisses;

    public bool IsEnabled => !_disposed;
    public string Name => "Redis Distributed Storage";
    public string Version => "1.0.0";
    public bool IsConnected => _redis?.IsConnected ?? false;

    public RedisDistributedStorage(
        IConnectionMultiplexer redis,
        ILogger<RedisDistributedStorage> logger,
        IOptions<DistributedStorageOptions> options)
    {
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _semaphore = new SemaphoreSlim(1, 1);
        _cache = new ConcurrentDictionary<string, byte[]>();
        _backupLock = new SemaphoreSlim(1, 1);

        if (_options.EnableBackup)
        {
            _backupTimer = new System.Timers.Timer(_options.BackupInterval.TotalMilliseconds);
            _backupTimer.Elapsed += async (s, e) => await BackupDataAsync();
            _backupTimer.Start();
        }
    }

    public async Task ConnectAsync()
    {
        ThrowIfDisposed();
        if (!_redis.IsConnected)
        {
            await _redis.GetDatabase().PingAsync();
        }
    }

    public async Task DisconnectAsync()
    {
        if (_redis.IsConnected)
        {
            await _redis.CloseAsync();
        }
    }

    public async Task<List<ErrorPattern>> LoadPatternsAsync()
    {
        ThrowIfDisposed();
        var patterns = new List<ErrorPattern>();

        try
        {
            await _semaphore.WaitAsync();
            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_redis.GetEndPoints().First());

            await foreach (var key in server.KeysAsync(pattern: $"{_options.KeyPrefix}pattern:*"))
            {
                if (_options.EnableDistributedCache && _cache.TryGetValue(key.ToString(), out var cachedValue))
                {
                    Interlocked.Increment(ref _cacheHits);
                    var pattern = JsonSerializer.Deserialize<ErrorPattern>(cachedValue);
                    if (pattern != null)
                    {
                        patterns.Add(pattern);
                    }
                }
                else
                {
                    Interlocked.Increment(ref _cacheMisses);
                    var value = await db.StringGetAsync(key);
                    if (value.HasValue)
                    {
                        var pattern = JsonSerializer.Deserialize<ErrorPattern>(value.ToString());
                        if (pattern != null)
                        {
                            patterns.Add(pattern);
                            if (_options.EnableDistributedCache)
                            {
                                _cache.TryAdd(key.ToString(), value);
                            }
                        }
                    }
                }
            }
        }
        finally
        {
            _semaphore.Release();
        }

        return patterns;
    }

    public async Task<ErrorPattern> GetPatternAsync(string patternId)
    {
        ArgumentNullException.ThrowIfNull(patternId);
        ThrowIfDisposed();

        var key = $"{_options.KeyPrefix}pattern:{patternId}";

        try
        {
            await _semaphore.WaitAsync();

            if (_options.EnableDistributedCache && _cache.TryGetValue(key, out var cachedValue))
            {
                Interlocked.Increment(ref _cacheHits);
                return JsonSerializer.Deserialize<ErrorPattern>(cachedValue);
            }

            Interlocked.Increment(ref _cacheMisses);
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync(key);

            if (!value.HasValue)
            {
                return null;
            }

            var pattern = JsonSerializer.Deserialize<ErrorPattern>(value.ToString());
            if (pattern != null && _options.EnableDistributedCache)
            {
                _cache.TryAdd(key, value);
            }

            return pattern;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task UpdatePatternAsync(ErrorPattern pattern)
    {
        ArgumentNullException.ThrowIfNull(pattern);
        ThrowIfDisposed();

        await StorePatternAsync(pattern);
    }

    public async Task DeletePatternAsync(string patternId)
    {
        ArgumentNullException.ThrowIfNull(patternId);
        ThrowIfDisposed();

        var key = $"{_options.KeyPrefix}pattern:{patternId}";

        try
        {
            await _semaphore.WaitAsync();
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(key);

            if (_options.EnableDistributedCache)
            {
                _cache.TryRemove(key, out _);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<StorageMetrics> GetMetricsAsync()
    {
        ThrowIfDisposed();

        try
        {
            await _semaphore.WaitAsync();
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var info = await server.InfoAsync();
            var memory = info.Single(x => x.Key == "Memory").Single(x => x.Key == "used_memory").Value;

            var totalHits = _cacheHits;
            var totalMisses = _cacheMisses;
            var hitRate = totalHits + totalMisses > 0
                ? (double)totalHits / (totalHits + totalMisses)
                : 0;

            return new StorageMetrics
            {
                TotalSizeBytes = long.Parse(memory),
                UsedSizeBytes = long.Parse(memory),
                AvailableSizeBytes = 0, // Not available from Redis info
                ItemCount = _cache.Count,
                CacheHitRate = hitRate,
                CacheMissRate = 1 - hitRate,
                Timestamp = DateTime.UtcNow,
                Metadata = new Dictionary<string, object>
                {
                    ["CacheHits"] = totalHits,
                    ["CacheMisses"] = totalMisses,
                    ["RedisConnected"] = _redis.IsConnected,
                    ["RedisEndpoint"] = _redis.GetEndPoints().First().ToString()
                }
            };
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task StoreContextAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        ThrowIfDisposed();

        var key = GetContextKey(context);
        var value = JsonSerializer.SerializeToUtf8Bytes(context);

        try
        {
            await _semaphore.WaitAsync();
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value, _options.ContextRetentionPeriod);

            if (_options.EnableDistributedCache)
            {
                _cache.AddOrUpdate(key, value, (_, _) => value);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error storing context for service {ServiceName}", context.ServiceName);
            throw new DistributedStorageException("Failed to store context", ex);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<ErrorContext>> GetContextsAsync(string serviceName, DateTime startTime, DateTime endTime)
    {
        ArgumentNullException.ThrowIfNull(serviceName);
        ThrowIfDisposed();

        var pattern = $"{_options.KeyPrefix}context:{serviceName}:*";
        var contexts = new List<ErrorContext>();

        try
        {
            await _semaphore.WaitAsync();
            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_redis.GetEndPoints().First());

            await foreach (var key in server.KeysAsync(pattern: pattern))
            {
                if (_options.EnableDistributedCache && _cache.TryGetValue(key.ToString(), out var cachedValue))
                {
                    var context = JsonSerializer.Deserialize<ErrorContext>(System.Text.Encoding.UTF8.GetString(cachedValue));
                    if (context != null && context.Timestamp >= startTime && context.Timestamp <= endTime)
                    {
                        contexts.Add(context);
                    }
                }
                else
                {
                    var value = await db.StringGetAsync(key);
                    if (value.HasValue)
                    {
                        var context = JsonSerializer.Deserialize<ErrorContext>(value.ToString());
                        if (context != null && context.Timestamp >= startTime && context.Timestamp <= endTime)
                        {
                            contexts.Add(context);
                            if (_options.EnableDistributedCache)
                            {
                                _cache.TryAdd(key.ToString(), System.Text.Encoding.UTF8.GetBytes(value.ToString()));
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving contexts for service {ServiceName}", serviceName);
            throw new DistributedStorageException("Failed to retrieve contexts", ex);
        }
        finally
        {
            _semaphore.Release();
        }

        return contexts;
    }

    public async Task StorePatternAsync(ErrorPattern pattern)
    {
        ArgumentNullException.ThrowIfNull(pattern);
        ThrowIfDisposed();

        var key = GetPatternKey(pattern);
        var value = JsonSerializer.SerializeToUtf8Bytes(pattern);

        try
        {
            await _semaphore.WaitAsync();
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value, _options.PatternRetentionPeriod);

            if (_options.EnableDistributedCache)
            {
                _cache.AddOrUpdate(key, value, (_, _) => value);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error storing pattern for service {ServiceName}", pattern.ServiceName);
            throw new DistributedStorageException("Failed to store pattern", ex);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<ErrorPattern>> GetPatternsAsync(string serviceName)
    {
        ArgumentNullException.ThrowIfNull(serviceName);
        ThrowIfDisposed();

        var pattern = $"{_options.KeyPrefix}pattern:{serviceName}:*";
        var patterns = new List<ErrorPattern>();

        try
        {
            await _semaphore.WaitAsync();
            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_redis.GetEndPoints().First());

            await foreach (var key in server.KeysAsync(pattern: pattern))
            {
                if (_options.EnableDistributedCache && _cache.TryGetValue(key.ToString(), out var cachedValue))
                {
                    var errorPattern = JsonSerializer.Deserialize<ErrorPattern>(cachedValue);
                    if (errorPattern != null)
                    {
                        patterns.Add(errorPattern);
                    }
                }
                else
                {
                    var value = await db.StringGetAsync(key);
                    if (value.HasValue)
                    {
                        var errorPattern = JsonSerializer.Deserialize<ErrorPattern>(value.ToString());
                        if (errorPattern != null)
                        {
                            patterns.Add(errorPattern);
                            if (_options.EnableDistributedCache)
                            {
                                _cache.TryAdd(key.ToString(), value);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patterns for service {ServiceName}", serviceName);
            throw new DistributedStorageException("Failed to retrieve patterns", ex);
        }
        finally
        {
            _semaphore.Release();
        }

        return patterns;
    }

    public async Task DeleteExpiredDataAsync()
    {
        ThrowIfDisposed();

        try
        {
            await _semaphore.WaitAsync();
            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_redis.GetEndPoints().First());

            // Delete expired contexts
            var contextPattern = $"{_options.KeyPrefix}context:*";
            await foreach (var key in server.KeysAsync(pattern: contextPattern))
            {
                var ttl = await db.KeyTimeToLiveAsync(key);
                if (!ttl.HasValue || ttl.Value <= TimeSpan.Zero)
                {
                    await db.KeyDeleteAsync(key);
                    if (_options.EnableDistributedCache)
                    {
                        _cache.TryRemove(key.ToString(), out _);
                    }
                }
            }

            // Delete expired patterns
            var patternPattern = $"{_options.KeyPrefix}pattern:*";
            await foreach (var key in server.KeysAsync(pattern: patternPattern))
            {
                var ttl = await db.KeyTimeToLiveAsync(key);
                if (!ttl.HasValue || ttl.Value <= TimeSpan.Zero)
                {
                    await db.KeyDeleteAsync(key);
                    if (_options.EnableDistributedCache)
                    {
                        _cache.TryRemove(key.ToString(), out _);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting expired data");
            throw new DistributedStorageException("Failed to delete expired data", ex);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task BackupDataAsync()
    {
        if (!_options.EnableBackup)
        {
            return;
        }

        try
        {
            await _backupLock.WaitAsync();
            var backupDir = Path.GetFullPath(_options.BackupPath);
            Directory.CreateDirectory(backupDir);

            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var backupFile = Path.Combine(backupDir, $"backup_{timestamp}.json");

            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var data = new Dictionary<string, byte[]>();

            await foreach (var key in server.KeysAsync(pattern: $"{_options.KeyPrefix}*"))
            {
                var value = await db.StringGetAsync(key);
                if (value.HasValue)
                {
                    data[key.ToString()] = value;
                }
            }

            await File.WriteAllTextAsync(backupFile, JsonSerializer.Serialize(data));

            // Clean up old backups
            var backupFiles = Directory.GetFiles(backupDir, "backup_*.json")
                .OrderByDescending(f => f)
                .Skip(_options.MaxBackupCount);

            foreach (var file in backupFiles)
            {
                File.Delete(file);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing backup");
        }
        finally
        {
            _backupLock.Release();
        }
    }

    private string GetContextKey(ErrorContext context)
    {
        return $"{_options.KeyPrefix}context:{context.ServiceName}:{context.Id}";
    }

    private string GetPatternKey(ErrorPattern pattern)
    {
        return $"{_options.KeyPrefix}pattern:{pattern.ServiceName}:{pattern.Id}";
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(RedisDistributedStorage));
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _semaphore?.Dispose();
                _backupLock?.Dispose();
                _backupTimer?.Dispose();
                _cache.Clear();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~RedisDistributedStorage()
    {
        Dispose(false);
    }
}
