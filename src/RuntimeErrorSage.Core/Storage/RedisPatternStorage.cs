using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Options;
using RuntimeErrorSage.Model.Storage.Exceptions;
using RuntimeErrorSage.Model.Storage.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Model.Storage;

/// <summary>
/// Redis-based implementation of pattern storage
/// </summary>
public class RedisPatternStorage : IPatternStorage
{
    private readonly ILogger<RedisPatternStorage> _logger;
    private readonly IConnectionMultiplexer _redis;
    private readonly string _keyPrefix;
    private bool _disposed;
    private readonly RedisPatternStorageOptions _options;

    public RedisPatternStorage(
        ILogger<RedisPatternStorage> logger,
        IConnectionMultiplexer redis,
        IOptions<RedisPatternStorageOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        _keyPrefix = options?.Value?.KeyPrefix ?? "pattern:";
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<List<ErrorPattern>> GetAllPatternsAsync()
    {
        try
        {
            var db = _redis.GetDatabase();
            var keys = await db.SetMembersAsync($"{_keyPrefix}all");
            var patterns = new List<ErrorPattern>();

            foreach (var key in keys)
            {
                var pattern = await GetPatternByIdAsync(key.ToString());
                if (pattern != null)
                {
                    patterns.Add(pattern);
                }
            }

            return patterns;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all patterns");
            throw;
        }
    }

    public async Task<ErrorPattern?> GetPatternByIdAsync(string id)
    {
        try
        {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync($"{_keyPrefix}{id}");
            
            if (value.IsNull)
            {
                return null;
            }

            return JsonSerializer.Deserialize<ErrorPattern>(value.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pattern by ID {PatternId}", id);
            throw;
        }
    }

    public async Task<List<ErrorPattern>> GetPatternsByTagAsync(string tag)
    {
        try
        {
            var db = _redis.GetDatabase();
            var keys = await db.SetMembersAsync($"{_keyPrefix}tag:{tag}");
            var patterns = new List<ErrorPattern>();

            foreach (var key in keys)
            {
                var pattern = await GetPatternByIdAsync(key.ToString());
                if (pattern != null)
                {
                    patterns.Add(pattern);
                }
            }

            return patterns;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting patterns by tag {Tag}", tag);
            throw;
        }
    }

    public async Task<List<ErrorPattern>> GetPatternsByCategoryAsync(string category)
    {
        try
        {
            var db = _redis.GetDatabase();
            var keys = await db.SetMembersAsync($"{_keyPrefix}category:{category}");
            var patterns = new List<ErrorPattern>();

            foreach (var key in keys)
            {
                var pattern = await GetPatternByIdAsync(key.ToString());
                if (pattern != null)
                {
                    patterns.Add(pattern);
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

    public async Task StorePatternAsync(string key, string pattern, TimeSpan? expiration = null)
    {
        try
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync($"{_keyPrefix}{key}", pattern, expiration);
            await db.SetAddAsync($"{_keyPrefix}all", key);
            _logger.LogDebug("Pattern stored with key {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error storing pattern with key {Key}", key);
            throw new PatternStorageException($"Failed to store pattern with key {key}", ex);
        }
    }

    public async Task<string> GetPatternAsync(string key)
    {
        try
        {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync($"{_keyPrefix}{key}");
            
            if (value.IsNull)
            {
                return string.Empty;
            }

            return value.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pattern with key {Key}", key);
            throw new PatternStorageException($"Failed to get pattern with key {key}", ex);
        }
    }

    public async Task RemovePatternAsync(string key)
    {
        try
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync($"{_keyPrefix}{key}");
            await db.SetRemoveAsync($"{_keyPrefix}all", key);
            _logger.LogDebug("Pattern removed with key {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing pattern with key {Key}", key);
            throw new PatternStorageException($"Failed to remove pattern with key {key}", ex);
        }
    }

    public async Task<bool> PatternExistsAsync(string key)
    {
        try
        {
            var db = _redis.GetDatabase();
            return await db.KeyExistsAsync($"{_keyPrefix}{key}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking pattern existence for key {Key}", key);
            throw new PatternStorageException($"Failed to check pattern existence for key {key}", ex);
        }
    }

    public async Task<Dictionary<string, string>> GetPatternsAsync(string pattern)
    {
        try
        {
            var db = _redis.GetDatabase();
            var keys = await db.SetMembersAsync($"{_keyPrefix}category:{pattern}");
            var result = new Dictionary<string, string>();

            foreach (var key in keys)
            {
                var patternValue = await GetPatternAsync(key.ToString());
                if (!string.IsNullOrEmpty(patternValue))
                {
                    result.Add(key.ToString(), patternValue);
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting patterns for category {Pattern}", pattern);
            throw new PatternStorageException($"Failed to get patterns for category {pattern}", ex);
        }
    }

    public async Task UpdateExpirationAsync(string key, TimeSpan expiration)
    {
        try
        {
            var db = _redis.GetDatabase();
            var exists = await db.KeyExistsAsync($"{_keyPrefix}{key}");
            
            if (exists)
            {
                await db.KeyExpireAsync($"{_keyPrefix}{key}", expiration);
                _logger.LogDebug("Updated expiration for pattern with key {Key}", key);
            }
            else
            {
                _logger.LogWarning("Pattern with key {Key} not found for expiration update", key);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating expiration for pattern with key {Key}", key);
            throw new PatternStorageException($"Failed to update expiration for pattern with key {key}", ex);
        }
    }

    public async Task ClearAllAsync()
    {
        try
        {
            var db = _redis.GetDatabase();
            var keys = await db.SetMembersAsync($"{_keyPrefix}all");
            
            foreach (var key in keys)
            {
                await db.KeyDeleteAsync($"{_keyPrefix}{key}");
            }
            
            await db.KeyDeleteAsync($"{_keyPrefix}all");
            _logger.LogInformation("All patterns cleared");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing all patterns");
            throw new PatternStorageException("Failed to clear all patterns", ex);
        }
    }

    public async Task<long> GetPatternCountAsync()
    {
        try
        {
            var db = _redis.GetDatabase();
            return await db.SetLengthAsync($"{_keyPrefix}all");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pattern count");
            throw new PatternStorageException("Failed to get pattern count", ex);
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

