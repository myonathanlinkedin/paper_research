using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Options;
using RuntimeErrorSage.Core.Storage.Exceptions;
using RuntimeErrorSage.Core.Storage.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.Storage;

/// <summary>
/// Redis-based implementation of pattern storage
/// </summary>
public class RedisPatternStorage : IPatternStorage
{
    private readonly ILogger<RedisPatternStorage> _logger;
    private readonly IConnectionMultiplexer _redis;
    private readonly string _keyPrefix;
    private bool _disposed;

    public RedisPatternStorage(
        ILogger<RedisPatternStorage> logger,
        IConnectionMultiplexer redis,
        IOptions<RedisPatternStorageOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        _keyPrefix = options?.Value?.KeyPrefix ?? "pattern:";
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