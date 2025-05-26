using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using CodeSage.Core.Options;
using CodeSage.Core.Interfaces;
using CodeSage.Core.Models;
using CodeSage.Core.Exceptions;
using CodeSage.Core.Interfaces.MCP;
using CodeSage.Core.Models.Error;
using CodeSage.Core.Models.MCP;
using System.Threading.Tasks;

namespace CodeSage.Core.MCP;

public class MCPClient : IMCPClient
{
    private readonly ILogger<MCPClient> _logger;
    private readonly IDistributedStorage _storage;
    private readonly ConcurrentDictionary<string, List<IContextSubscriber>> _subscribers;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly MCPClientOptions _options;
    private readonly HttpClient _httpClient;

    public MCPClient(
        ILogger<MCPClient> logger,
        IDistributedStorage storage,
        IOptions<MCPClientOptions> options,
        HttpClient httpClient)
    {
        _logger = logger;
        _storage = storage;
        _options = options.Value;
        _httpClient = httpClient;
        _subscribers = new();
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task PublishContextAsync(ErrorContext context, ErrorAnalysisResult? analysis = null)
    {
        try
        {
            // Store context in distributed storage
            await _storage.StoreContextAsync(context);

            // Notify local subscribers
            await NotifySubscribersAsync(context);

            // Cross-service communication if enabled
            if (_options.EnableCrossServiceCommunication)
            {
                await PublishToOtherServicesAsync(context);
            }

            // Update patterns
            await UpdateErrorPatternsAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing context for service {Service}", context.ServiceName);
            throw new MCPPublishingException("Failed to publish context", ex);
        }
    }

    public async Task<List<ErrorContext>> GetContextHistoryAsync(
        string serviceName,
        DateTime startTime,
        DateTime endTime)
    {
        try
        {
            return await _storage.GetContextsAsync(serviceName, startTime, endTime);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving context history for service {Service}", serviceName);
            throw new MCPRetrievalException("Failed to retrieve context history", ex);
        }
    }

    public async Task<List<ErrorPattern>> GetErrorPatternsAsync(string serviceName)
    {
        try
        {
            return await _storage.GetPatternsAsync(serviceName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving error patterns for service {Service}", serviceName);
            throw new MCPRetrievalException("Failed to retrieve error patterns", ex);
        }
    }

    public async Task SubscribeAsync(string serviceName, IContextSubscriber subscriber)
    {
        try
        {
            _subscribers.AddOrUpdate(
                serviceName,
                new List<IContextSubscriber> { subscriber },
                (_, list) =>
                {
                    if (!list.Contains(subscriber))
                    {
                        list.Add(subscriber);
                    }
                    return list;
                });

            // Register for cross-service updates if enabled
            if (_options.EnableCrossServiceCommunication)
            {
                await RegisterCrossServiceSubscriptionAsync(serviceName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error subscribing to service {Service}", serviceName);
            throw new MCPSubscriptionException("Failed to subscribe to service", ex);
        }
    }

    public async Task UnsubscribeAsync(string serviceName, IContextSubscriber subscriber)
    {
        try
        {
            if (_subscribers.TryGetValue(serviceName, out var subscribers))
            {
                subscribers.Remove(subscriber);
            }

            // Unregister from cross-service updates if enabled
            if (_options.EnableCrossServiceCommunication)
            {
                await UnregisterCrossServiceSubscriptionAsync(serviceName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unsubscribing from service {Service}", serviceName);
            throw new MCPSubscriptionException("Failed to unsubscribe from service", ex);
        }
    }

    public async Task UpdateErrorPatternsAsync(string serviceName, List<ErrorPattern> patterns)
    {
        try
        {
            foreach (var pattern in patterns)
            {
                await _storage.StorePatternAsync(pattern);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating error patterns for service {Service}", serviceName);
            throw new MCPUpdateException("Failed to update error patterns", ex);
        }
    }

    public async Task DeleteErrorPatternsAsync(string serviceName, List<string> patternIds)
    {
        try
        {
            // DistributedStorage currently doesn't support deleting patterns by ID
            // This would need to be implemented in the distributed storage layer
            _logger.LogWarning("DeleteErrorPatternsAsync not fully implemented in DistributedStorage");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting error patterns for service {Service}", serviceName);
            throw new MCPDeletionException("Failed to delete error patterns", ex);
        }
    }

    public async Task<MCPConnectionStatus> GetConnectionStatusAsync()
    {
        // This would typically check the actual connection status to the MCP server
        return await Task.FromResult(new MCPConnectionStatus { IsConnected = true });
    }

    public async Task<bool> IsConnectedAsync()
    {
        // This would typically check the actual connection status to the MCP server
        return await Task.FromResult(true);
    }

    public async Task DisconnectAsync()
    {
        // Implement disconnection logic
        _logger.LogInformation("MCP client disconnected");
        await Task.CompletedTask;
    }

    public async Task ReconnectAsync()
    {
        // Implement reconnection logic
        _logger.LogInformation("Attempting to reconnect MCP client");
        await Task.CompletedTask;
    }

    private async Task NotifySubscribersAsync(ErrorContext context)
    {
        if (_subscribers.TryGetValue(context.ServiceName, out var serviceSubscribers))
        {
            // Assuming ErrorAnalysisResult is part of the context when publishing
            var analysis = context.Analysis;

            if (analysis != null)
            {
                foreach (var subscriber in serviceSubscribers)
                {
                    try
                    {
                        await subscriber.HandleContextUpdateAsync(context, analysis);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error notifying subscriber for service {Service}", context.ServiceName);
                    }
                }
            }
        }
    }

    private async Task PublishToOtherServicesAsync(ErrorContext context)
    {
        // This is a placeholder for actual cross-service communication
        _logger.LogInformation("Publishing context to other services is not fully implemented");
        await Task.CompletedTask;
    }

    private async Task RegisterCrossServiceSubscriptionAsync(string serviceName)
    {
        // This is a placeholder for actual cross-service subscription registration
        _logger.LogInformation("Registering cross-service subscription is not fully implemented");
        await Task.CompletedTask;
    }

    private async Task UnregisterCrossServiceSubscriptionAsync(string serviceName)
    {
        // This is a placeholder for actual cross-service subscription unregistration
        _logger.LogInformation("Unregistering cross-service subscription is not fully implemented");
        await Task.CompletedTask;
    }

    private async Task UpdateErrorPatternsAsync(ErrorContext context)
    {
        try
        {
            var pattern = new ErrorPattern
            {
                ServiceName = context.ServiceName,
                ErrorType = context.Exception?.GetType().Name ?? "Unknown",
                OperationName = context.OperationName,
                FirstOccurrence = context.Timestamp,
                LastOccurrence = context.Timestamp,
                OccurrenceCount = 1,
                Context = context
            };

            await _storage.StorePatternAsync(pattern);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating error patterns for service {Service}", context.ServiceName);
            // Don't throw - pattern updates shouldn't block context publishing
        }
    }
}

public class MCPPublishingException : Exception
{
    public MCPPublishingException(string message, Exception inner) : base(message, inner) { }
}

public class MCPRetrievalException : Exception
{
    public MCPRetrievalException(string message, Exception inner) : base(message, inner) { }
}

public class MCPSubscriptionException : Exception
{
    public MCPSubscriptionException(string message, Exception inner) : base(message, inner) { }
} 