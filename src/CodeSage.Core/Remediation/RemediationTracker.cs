using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;
using CodeSage.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeSage.Core.Remediation.Models.Execution;

namespace CodeSage.Core.Remediation;

public class RemediationTrackerOptions
{
    public string ConnectionString { get; set; } = "localhost:6379";
    public string KeyPrefix { get; set; } = "remediation:";
    public TimeSpan ExecutionRetentionPeriod { get; set; } = TimeSpan.FromDays(30);
    public int MaxStoredExecutions { get; set; } = 1000;
}

public class RemediationTracker : IRemediationTracker, IDisposable
{
    private readonly ILogger<RemediationTracker> _logger;
    private readonly RemediationTrackerOptions _options;
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly Timer _cleanupTimer;
    private readonly Dictionary<string, RemediationExecution> _executions;

    public RemediationTracker(
        ILogger<RemediationTracker> logger,
        IOptions<RemediationTrackerOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _redis = ConnectionMultiplexer.Connect(_options.ConnectionString);
        _db = _redis.GetDatabase();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        // Setup cleanup timer
        _cleanupTimer = new Timer(
            async _ => await CleanupExpiredExecutionsAsync(),
            null,
            TimeSpan.FromHours(1),
            TimeSpan.FromHours(1));

        _executions = new Dictionary<string, RemediationExecution>();
    }

    public async Task TrackExecutionAsync(RemediationExecution execution)
    {
        try
        {
            _logger.LogInformation(
                "Tracking remediation execution {ExecutionId} for error {ErrorType}",
                execution.ExecutionId,
                execution.Context.ErrorType);

            // Update execution end time if completed
            if (execution.Status is RemediationStatus.Completed or RemediationStatus.Failed or RemediationStatus.Partial)
            {
                execution.EndTime = DateTime.UtcNow;
            }

            // Serialize execution
            var executionJson = JsonSerializer.Serialize(execution, _jsonOptions);

            // Store execution
            var key = GetExecutionKey(execution.ExecutionId);
            await _db.StringSetAsync(
                key,
                executionJson,
                _options.ExecutionRetentionPeriod);

            // Add to service index
            var serviceKey = GetServiceExecutionsKey(execution.Context.ServiceName);
            await _db.SortedSetAddAsync(
                serviceKey,
                execution.ExecutionId,
                execution.StartTime.ToUnixTime());

            // Add to error type index
            var errorKey = GetErrorTypeExecutionsKey(execution.Context.ErrorType);
            await _db.SortedSetAddAsync(
                errorKey,
                execution.ExecutionId,
                execution.StartTime.ToUnixTime());

            // Trim indices if needed
            await TrimIndicesAsync(serviceKey);
            await TrimIndicesAsync(errorKey);

            _logger.LogInformation(
                "Successfully tracked remediation execution {ExecutionId}",
                execution.ExecutionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error tracking remediation execution {ExecutionId}",
                execution.ExecutionId);
            throw new RemediationTrackingException(
                $"Failed to track remediation execution {execution.ExecutionId}",
                ex);
        }
    }

    public async Task<RemediationExecution?> GetExecutionAsync(string executionId)
    {
        try
        {
            _logger.LogInformation(
                "Retrieving remediation execution {ExecutionId}",
                executionId);

            var key = GetExecutionKey(executionId);
            var executionJson = await _db.StringGetAsync(key);

            if (executionJson.IsNull)
            {
                _logger.LogWarning(
                    "Remediation execution {ExecutionId} not found",
                    executionId);
                return null;
            }

            var execution = JsonSerializer.Deserialize<RemediationExecution>(
                executionJson!,
                _jsonOptions);

            _logger.LogInformation(
                "Successfully retrieved remediation execution {ExecutionId}",
                executionId);

            return execution;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving remediation execution {ExecutionId}",
                executionId);
            throw new RemediationTrackingException(
                $"Failed to retrieve remediation execution {executionId}",
                ex);
        }
    }

    public async Task<IEnumerable<RemediationExecution>> GetServiceExecutionsAsync(
        string serviceName,
        DateTime startTime,
        DateTime endTime)
    {
        try
        {
            _logger.LogInformation(
                "Retrieving remediation executions for service {ServiceName} between {StartTime} and {EndTime}",
                serviceName,
                startTime,
                endTime);

            var serviceKey = GetServiceExecutionsKey(serviceName);
            var executionIds = await _db.SortedSetRangeByScoreAsync(
                serviceKey,
                startTime.ToUnixTime(),
                endTime.ToUnixTime());

            var executions = new List<RemediationExecution>();
            foreach (var executionId in executionIds)
            {
                var execution = await GetExecutionAsync(executionId.ToString());
                if (execution != null)
                {
                    executions.Add(execution);
                }
            }

            _logger.LogInformation(
                "Successfully retrieved {Count} remediation executions for service {ServiceName}",
                executions.Count,
                serviceName);

            return executions;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving remediation executions for service {ServiceName}",
                serviceName);
            throw new RemediationTrackingException(
                $"Failed to retrieve remediation executions for service {serviceName}",
                ex);
        }
    }

    public async Task<IEnumerable<RemediationExecution>> GetErrorTypeExecutionsAsync(
        string errorType,
        DateTime startTime,
        DateTime endTime)
    {
        try
        {
            _logger.LogInformation(
                "Retrieving remediation executions for error type {ErrorType} between {StartTime} and {EndTime}",
                errorType,
                startTime,
                endTime);

            var errorKey = GetErrorTypeExecutionsKey(errorType);
            var executionIds = await _db.SortedSetRangeByScoreAsync(
                errorKey,
                startTime.ToUnixTime(),
                endTime.ToUnixTime());

            var executions = new List<RemediationExecution>();
            foreach (var executionId in executionIds)
            {
                var execution = await GetExecutionAsync(executionId.ToString());
                if (execution != null)
                {
                    executions.Add(execution);
                }
            }

            _logger.LogInformation(
                "Successfully retrieved {Count} remediation executions for error type {ErrorType}",
                executions.Count,
                errorType);

            return executions;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving remediation executions for error type {ErrorType}",
                errorType);
            throw new RemediationTrackingException(
                $"Failed to retrieve remediation executions for error type {errorType}",
                ex);
        }
    }

    public async Task<RemediationStatistics> GetStatisticsAsync(
        string? serviceName = null,
        string? errorType = null,
        DateTime? startTime = null,
        DateTime? endTime = null)
    {
        try
        {
            _logger.LogInformation(
                "Retrieving remediation statistics for service {ServiceName} and error type {ErrorType}",
                serviceName,
                errorType);

            var statistics = new RemediationStatistics();
            var executions = new List<RemediationExecution>();

            // Get executions based on filters
            if (serviceName != null)
            {
                executions.AddRange(await GetServiceExecutionsAsync(
                    serviceName,
                    startTime ?? DateTime.MinValue,
                    endTime ?? DateTime.MaxValue));
            }
            else if (errorType != null)
            {
                executions.AddRange(await GetErrorTypeExecutionsAsync(
                    errorType,
                    startTime ?? DateTime.MinValue,
                    endTime ?? DateTime.MaxValue));
            }

            // Calculate statistics
            statistics.TotalExecutions = executions.Count;
            statistics.SuccessfulExecutions = executions.Count(e => e.Status == RemediationStatus.Completed);
            statistics.FailedExecutions = executions.Count(e => e.Status == RemediationStatus.Failed);
            statistics.PartialExecutions = executions.Count(e => e.Status == RemediationStatus.Partial);
            statistics.SkippedExecutions = executions.Count(e => e.Status == RemediationStatus.Skipped);

            if (executions.Any())
            {
                statistics.AverageExecutionTime = TimeSpan.FromTicks(
                    (long)executions
                        .Where(e => e.EndTime.HasValue)
                        .Average(e => (e.EndTime!.Value - e.StartTime).Ticks));

                statistics.SuccessRate = (double)statistics.SuccessfulExecutions / statistics.TotalExecutions;
            }

            _logger.LogInformation(
                "Successfully retrieved remediation statistics: {Statistics}",
                JsonSerializer.Serialize(statistics, _jsonOptions));

            return statistics;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving remediation statistics for service {ServiceName} and error type {ErrorType}",
                serviceName,
                errorType);
            throw new RemediationTrackingException(
                "Failed to retrieve remediation statistics",
                ex);
        }
    }

    private async Task CleanupExpiredExecutionsAsync()
    {
        try
        {
            _logger.LogInformation("Starting cleanup of expired remediation executions");

            // Get all service keys
            var serviceKeys = await _db.SetMembersAsync("remediation:services");
            foreach (var serviceKey in serviceKeys)
            {
                await CleanupServiceExecutionsAsync(serviceKey.ToString());
            }

            // Get all error type keys
            var errorKeys = await _db.SetMembersAsync("remediation:errorTypes");
            foreach (var errorKey in errorKeys)
            {
                await CleanupErrorTypeExecutionsAsync(errorKey.ToString());
            }

            _logger.LogInformation("Successfully completed cleanup of expired remediation executions");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during cleanup of expired remediation executions");
        }
    }

    private async Task CleanupServiceExecutionsAsync(string serviceName)
    {
        var serviceKey = GetServiceExecutionsKey(serviceName);
        var cutoffTime = DateTime.UtcNow.Subtract(_options.ExecutionRetentionPeriod).ToUnixTime();

        var expiredIds = await _db.SortedSetRangeByScoreAsync(
            serviceKey,
            0,
            cutoffTime);

        foreach (var executionId in expiredIds)
        {
            var key = GetExecutionKey(executionId.ToString());
            await _db.KeyDeleteAsync(key);
            await _db.SortedSetRemoveAsync(serviceKey, executionId);
        }
    }

    private async Task CleanupErrorTypeExecutionsAsync(string errorType)
    {
        var errorKey = GetErrorTypeExecutionsKey(errorType);
        var cutoffTime = DateTime.UtcNow.Subtract(_options.ExecutionRetentionPeriod).ToUnixTime();

        var expiredIds = await _db.SortedSetRangeByScoreAsync(
            errorKey,
            0,
            cutoffTime);

        foreach (var executionId in expiredIds)
        {
            var key = GetExecutionKey(executionId.ToString());
            await _db.KeyDeleteAsync(key);
            await _db.SortedSetRemoveAsync(errorKey, executionId);
        }
    }

    private async Task TrimIndicesAsync(string key)
    {
        if (await _db.SortedSetLengthAsync(key) > _options.MaxStoredExecutions)
        {
            await _db.SortedSetRemoveRangeByRankAsync(
                key,
                0,
                -_options.MaxStoredExecutions - 1);
        }
    }

    private string GetExecutionKey(string executionId) =>
        $"{_options.KeyPrefix}execution:{executionId}";

    private string GetServiceExecutionsKey(string serviceName) =>
        $"{_options.KeyPrefix}service:{serviceName}";

    private string GetErrorTypeExecutionsKey(string errorType) =>
        $"{_options.KeyPrefix}errorType:{errorType}";

    public void Dispose()
    {
        _cleanupTimer.Dispose();
        _redis.Dispose();
    }

    public async Task TrackRemediationAsync(RemediationExecution execution)
    {
        // Implementation here
        await Task.CompletedTask;
    }

    public async Task<RemediationExecution?> GetRemediationAsync(string remediationId)
    {
        // Implementation here
        return await Task.FromResult<RemediationExecution?>(null);
    }
}

public class RemediationStatistics
{
    public int TotalExecutions { get; set; }
    public int SuccessfulExecutions { get; set; }
    public int FailedExecutions { get; set; }
    public int PartialExecutions { get; set; }
    public int SkippedExecutions { get; set; }
    public double SuccessRate { get; set; }
    public TimeSpan AverageExecutionTime { get; set; }
}

public class RemediationTrackingException : Exception
{
    public RemediationTrackingException(string message, Exception inner) : base(message, inner) { }
    public RemediationTrackingException(string message) : base(message) { }
}

public static class DateTimeExtensions
{
    public static double ToUnixTime(this DateTime dateTime) =>
        (dateTime - DateTime.UnixEpoch).TotalSeconds;
} 