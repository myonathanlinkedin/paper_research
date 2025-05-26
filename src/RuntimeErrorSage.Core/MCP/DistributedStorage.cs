using StackExchange.Redis;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Timers;
using System.IO;
using System.Linq;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.LLM.Options;
using RuntimeErrorSage.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.MCP;

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

public class RedisDistributedStorage : IDistributedStorage, IDisposable
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<RedisDistributedStorage> _logger;
    private readonly DistributedStorageOptions _options;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly ConcurrentDictionary<string, object> _localCache;
    private readonly System.Timers.Timer _backupTimer;
    private readonly SemaphoreSlim _backupLock = new(1, 1);
    private readonly Random _partitionRandom = new();

    public RedisDistributedStorage(
        IConnectionMultiplexer redis,
        ILogger<RedisDistributedStorage> logger,
        IOptions<DistributedStorageOptions> options)
    {
        _redis = redis;
        _logger = logger;
        _options = options.Value;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _localCache = new();
        
        if (_options.EnableBackup)
        {
            Directory.CreateDirectory(_options.BackupPath);
            _backupTimer = new System.Timers.Timer(
                PerformBackupAsync,
                null,
                _options.BackupInterval,
                _options.BackupInterval);
        }
    }

    public async Task StoreContextAsync(ErrorContext context)
    {
        _logger.LogInformation("Storing error context for service {Service}", context.ServiceName);
        try
        {
            var db = _redis.GetDatabase(GetPartitionIndex(context.ServiceName));
            var key = GetContextKey(context);
            var value = JsonSerializer.Serialize(context, _jsonOptions);

            // Store in Redis with expiration
            await db.StringSetAsync(
                key,
                value,
                _options.ContextRetentionPeriod,
                When.Always);

            // Update service index
            await UpdateServiceIndexAsync(context, db);

            // Update local cache if enabled
            if (_options.EnableDistributedCache)
            {
                UpdateLocalCache(key, value);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error storing context for service {Service}", context.ServiceName);
            throw new DistributedStorageException("Failed to store context", ex);
        }
    }

    public async Task<List<ErrorContext>> GetContextsAsync(
        string serviceName,
        DateTime startTime,
        DateTime endTime)
    {
        _logger.LogInformation("Retrieving error contexts for service {Service}", serviceName);
        try
        {
            var db = _redis.GetDatabase(GetPartitionIndex(serviceName));
            var serviceKey = GetServiceKey(serviceName);
            var contexts = new List<ErrorContext>();

            // Get correlation IDs within time range
            var correlationIds = await db.SortedSetRangeByScoreAsync(
                serviceKey,
                startTime.ToUnixTimeSeconds(),
                endTime.ToUnixTimeSeconds());

            foreach (var correlationId in correlationIds)
            {
                var key = GetContextKey(serviceName, correlationId.ToString()!);
                string? value;

                // Try local cache first if enabled
                if (_options.EnableDistributedCache && _localCache.TryGetValue(key, out var cachedValue))
                {
                    value = cachedValue.ToString();
                }
                else
                {
                    value = await db.StringGetAsync(key);
                    if (!value.IsNull && _options.EnableDistributedCache)
                    {
                        UpdateLocalCache(key, value!);
                    }
                }

                if (!string.IsNullOrEmpty(value))
                {
                    var context = JsonSerializer.Deserialize<ErrorContext>(value, _jsonOptions);
                    if (context != null)
                    {
                        contexts.Add(context);
                    }
                }
            }

            return contexts.OrderByDescending(c => c.Timestamp).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving contexts for service {Service}", serviceName);
            throw new DistributedStorageException("Failed to retrieve contexts", ex);
        }
    }

    public async Task StorePatternAsync(ErrorPattern pattern)
    {
        _logger.LogInformation("Storing error pattern {PatternId} for service {Service}", pattern.PatternId, pattern.ServiceName);
        try
        {
            await _semaphore.WaitAsync();
            try
            {
                var db = _redis.GetDatabase(GetPartitionIndex(pattern.ServiceName));
                var key = GetPatternKey(pattern);
                var value = JsonSerializer.Serialize(pattern, _jsonOptions);

                // Use transaction for atomic updates
                var tran = db.CreateTransaction();
                tran.StringSetAsync(key, value, _options.PatternRetentionPeriod);
                
                // Update pattern index
                var indexKey = GetPatternIndexKey(pattern.ServiceName);
                tran.SortedSetAddAsync(
                    indexKey,
                    $"{pattern.ErrorType}:{pattern.OperationName}",
                    pattern.LastOccurrence.ToUnixTimeSeconds());

                await tran.ExecuteAsync();

                // Update local cache if enabled
                if (_options.EnableDistributedCache)
                {
                    UpdateLocalCache(key, value);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error storing pattern for service {Service}", pattern.ServiceName);
            throw new DistributedStorageException("Failed to store pattern", ex);
        }
    }

    public async Task<List<ErrorPattern>> GetPatternsAsync(string serviceName)
    {
        _logger.LogInformation("Retrieving error patterns for service {Service}", serviceName);
        try
        {
            var db = _redis.GetDatabase();
            var indexKey = $"{_options.KeyPrefix}service:{serviceName}:patterns";

            // Get all pattern keys
            var patternKeys = await db.SortedSetRangeByScoreAsync(
                indexKey,
                DateTime.UtcNow.Add(-_options.PatternRetentionPeriod).ToUnixTimeSeconds(),
                DateTime.UtcNow.ToUnixTimeSeconds());

            var patterns = new List<ErrorPattern>();
            foreach (var patternKey in patternKeys)
            {
                var key = $"{_options.KeyPrefix}pattern:{serviceName}:{patternKey}";
                var value = await db.StringGetAsync(key);
                
                if (!value.IsNull)
                {
                    var pattern = JsonSerializer.Deserialize<ErrorPattern>(value!, _jsonOptions);
                    if (pattern != null)
                    {
                        patterns.Add(pattern);
                    }
                }
            }

            return patterns.OrderByDescending(p => p.OccurrenceCount).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patterns for service {Service}", serviceName);
            throw new DistributedStorageException("Failed to retrieve patterns", ex);
        }
    }

    public async Task DeleteExpiredDataAsync()
    {
        _logger.LogInformation("Deleting expired data");
        try
        {
            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_redis.GetEndPoints().First());

            // Delete expired contexts
            var contextKeys = server.Keys(pattern: $"{_options.KeyPrefix}context:*");
            foreach (var key in contextKeys)
            {
                var ttl = await db.KeyTimeToLiveAsync(key);
                if (ttl == null || ttl.Value.TotalSeconds <= 0)
                {
                    await db.KeyDeleteAsync(key);
                }
            }

            // Delete expired patterns
            var patternKeys = server.Keys(pattern: $"{_options.KeyPrefix}pattern:*");
            foreach (var key in patternKeys)
            {
                var ttl = await db.KeyTimeToLiveAsync(key);
                if (ttl == null || ttl.Value.TotalSeconds <= 0)
                {
                    await db.KeyDeleteAsync(key);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting expired data");
            throw new DistributedStorageException("Failed to delete expired data", ex);
        }
    }

    private async Task PerformBackupAsync(object? state)
    {
        if (!await _backupLock.WaitAsync(0)) // Try to acquire lock without waiting
        {
            _logger.LogWarning("Backup already in progress, skipping this backup cycle");
            return;
        }

        try
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var backupFile = Path.Combine(_options.BackupPath, $"backup_{timestamp}.rdb");

            // Get Redis server
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            
            // Perform backup
            await server.BackupAsync(backupFile);

            // Cleanup old backups
            var backupFiles = Directory.GetFiles(_options.BackupPath, "backup_*.rdb")
                .OrderByDescending(f => f)
                .Skip(_options.MaxBackupCount);

            foreach (var file in backupFiles)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting old backup file {File}", file);
                }
            }

            _logger.LogInformation("Backup completed successfully: {File}", backupFile);
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

    private void UpdateLocalCache(string key, string value)
    {
        // Implement LRU cache eviction
        if (_localCache.Count >= _options.CacheMaxSize)
        {
            var oldestKey = _localCache.Keys.First();
            _localCache.TryRemove(oldestKey, out _);
        }

        _localCache[key] = value;
    }

    private int GetPartitionIndex(string serviceName)
    {
        if (!_options.EnableDataPartitioning)
        {
            return 0;
        }

        // Use consistent hashing for partition selection
        var hash = Math.Abs(serviceName.GetHashCode());
        return hash % _options.PartitionCount;
    }

    private string GetContextKey(ErrorContext context) =>
        $"{_options.KeyPrefix}context:{context.ServiceName}:{context.CorrelationId}";

    private string GetContextKey(string serviceName, string correlationId) =>
        $"{_options.KeyPrefix}context:{serviceName}:{correlationId}";

    private string GetServiceKey(string serviceName) =>
        $"{_options.KeyPrefix}service:{serviceName}:contexts";

    private string GetPatternKey(ErrorPattern pattern) =>
        $"{_options.KeyPrefix}pattern:{pattern.ServiceName}:{pattern.ErrorType}:{pattern.OperationName}";

    private string GetPatternIndexKey(string serviceName) =>
        $"{_options.KeyPrefix}service:{serviceName}:patterns";

    private async Task UpdateServiceIndexAsync(ErrorContext context, IDatabase db)
    {
        var serviceKey = GetServiceKey(context.ServiceName);
        await db.SortedSetAddAsync(
            serviceKey,
            context.CorrelationId,
            context.Timestamp.ToUnixTimeSeconds());

        // Trim old contexts
        await db.SortedSetRemoveRangeByScoreAsync(
            serviceKey,
            0,
            DateTime.UtcNow.Add(-_options.ContextRetentionPeriod).ToUnixTimeSeconds());
    }

    public void Dispose()
    {
        _semaphore.Dispose();
        _backupTimer?.Dispose();
        _backupLock.Dispose();
    }
}

public class DistributedStorageException : Exception
{
    public DistributedStorageException(string message, Exception inner) : base(message, inner) { }
}

public static class DateTimeExtensions
{
    public static long ToUnixTimeSeconds(this DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }
} 
