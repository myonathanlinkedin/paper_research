using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using RuntimeErrorSage.Application.Storage;
using RuntimeErrorSage.Application.Models.Health;

namespace RuntimeErrorSage.Application.Health;

/// <summary>
/// Health check for Redis connection.
/// </summary>
public class RedisHealthCheck : IHealthCheck
{
    private readonly IConnectionMultiplexer _redis;
    private readonly RedisPatternStorageOptions _options;

    public RedisHealthCheck(
        IConnectionMultiplexer redis,
        IOptions<RedisPatternStorageOptions> options)
    {
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_redis.IsConnected)
            {
                return HealthCheckResult.Unhealthy("Redis is not connected");
            }

            var db = _redis.GetDatabase();
            var testKey = $"{_options.KeyPrefix}health:test";
            await db.StringSetAsync(testKey, "1", TimeSpan.FromSeconds(1));
            var value = await db.StringGetAsync(testKey);

            return value == "1"
                ? HealthCheckResult.Healthy("Redis is healthy")
                : HealthCheckResult.Degraded("Redis test write/read failed");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Redis health check failed", ex);
        }
    }
} 
