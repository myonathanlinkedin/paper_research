using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Interfaces.MCP;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Remediation.Models.Common;
using ContextHistory = RuntimeErrorSage.Core.Models.Common.ContextHistory;
using TimeRange = RuntimeErrorSage.Core.Models.Common.TimeRange;

namespace RuntimeErrorSage.Core.MCP;

/// <summary>
/// Implements the Model Context Protocol (MCP) client for error context analysis.
/// </summary>
public class MCPClient : IMCPClient
{
    private readonly ILogger<MCPClient> _logger;
    private readonly string _clientId;
    private readonly ConnectionStatus _connectionStatus;
    private readonly Dictionary<string, ErrorContext> _contextCache;
    private readonly Dictionary<string, List<ContextHistory>> _contextHistory;
    private readonly Dictionary<string, Func<ErrorContext, Task>> _contextSubscribers;

    /// <summary>
    /// Initializes a new instance of the MCPClient class.
    /// </summary>
    public MCPClient(ILogger<MCPClient> logger, string clientId)
    {
        _logger = logger;
        _clientId = clientId;
        _connectionStatus = new ConnectionStatus
        {
            IsConnected = false,
            LastConnected = null,
            LastDisconnected = null,
            Status = ConnectionState.Disconnected,
            ErrorMessage = null,
            Details = new Dictionary<string, object>()
        };
        _contextCache = new Dictionary<string, ErrorContext>();
        _contextHistory = new Dictionary<string, List<ContextHistory>>();
        _contextSubscribers = new Dictionary<string, Func<ErrorContext, Task>>();
    }

    /// <summary>
    /// Gets the client identifier.
    /// </summary>
    public string ClientId => _clientId;

    /// <summary>
    /// Gets a value indicating whether the client is connected.
    /// </summary>
    public bool IsConnected => _connectionStatus.IsConnected;

    /// <summary>
    /// Gets the current connection status.
    /// </summary>
    public ConnectionStatus ConnectionStatus => _connectionStatus;

    /// <summary>
    /// Connects to the MCP server.
    /// </summary>
    public async Task ConnectAsync()
    {
        try
        {
            _connectionStatus.Status = ConnectionState.Connecting;
            _connectionStatus.LastConnected = DateTime.UtcNow;
            _connectionStatus.IsConnected = true;
            _connectionStatus.Status = ConnectionState.Connected;
            _connectionStatus.ErrorMessage = null;
            _connectionStatus.Details["ConnectedAt"] = _connectionStatus.LastConnected;
        }
        catch (Exception ex)
        {
            _connectionStatus.Status = ConnectionState.Failed;
            _connectionStatus.IsConnected = false;
            _connectionStatus.ErrorMessage = ex.Message;
            _connectionStatus.Details["Error"] = ex.ToString();
            _logger.LogError(ex, "Failed to connect MCP client");
            throw;
        }
    }

    /// <summary>
    /// Analyzes an error context.
    /// </summary>
    /// <param name="context">The error context to analyze.</param>
    /// <returns>The analyzed error context.</returns>
    public async Task<ErrorContext> AnalyzeErrorAsync(ErrorContext context)
    {
        try
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("MCP client is not connected");
            }

            // Cache the context
            _contextCache[context.ErrorId] = context;

            // Record context history
            var history = _contextHistory.GetOrAdd(context.ErrorId, _ => new List<ContextHistory>());
            history.Add(new ContextHistory
            {
                Id = Guid.NewGuid().ToString(),
                ContextId = context.ErrorId,
                Timestamp = DateTime.UtcNow,
                ChangeType = ContextChangeType.Analyzed,
                PreviousState = null,
                NewState = context,
                ChangedBy = _clientId,
                Metadata = new Dictionary<string, object>()
            });

            // Notify subscribers
            if (_contextSubscribers.TryGetValue(context.ErrorId, out var callback))
            {
                await callback(context);
            }

            return context;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing context {ErrorId}", context.ErrorId);
            throw;
        }
    }

    /// <summary>
    /// Gets the context history for a specified time range.
    /// </summary>
    /// <param name="contextId">The context identifier.</param>
    /// <param name="range">The time range.</param>
    /// <returns>The context history.</returns>
    public async Task<List<ContextHistory>> GetContextHistoryAsync(string contextId, TimeRange range)
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("MCP client is not connected");
        }

        if (_contextHistory.TryGetValue(contextId, out var history))
        {
            return history
                .Where(h => h.Timestamp >= range.Start && h.Timestamp <= range.End)
                .ToList();
        }

        return new List<ContextHistory>();
    }

    /// <summary>
    /// Gets the connection status for a client.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <returns>The connection status.</returns>
    public async Task<ConnectionStatus> GetConnectionStatusAsync(string clientId)
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("MCP client is not connected");
        }

        if (clientId != _clientId)
        {
            throw new ArgumentException("Invalid client ID", nameof(clientId));
        }

        return _connectionStatus;
    }

    /// <summary>
    /// Subscribes to context updates.
    /// </summary>
    /// <param name="contextId">The context identifier.</param>
    /// <param name="callback">The callback to invoke when the context is updated.</param>
    public async Task SubscribeToContextUpdatesAsync(string contextId, Func<ErrorContext, Task> callback)
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("MCP client is not connected");
        }

        _contextSubscribers[contextId] = callback;
    }

    /// <summary>
    /// Unsubscribes from context updates.
    /// </summary>
    /// <param name="contextId">The context identifier.</param>
    public async Task UnsubscribeFromContextUpdatesAsync(string contextId)
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("MCP client is not connected");
        }

        _contextSubscribers.Remove(contextId);
    }

    /// <summary>
    /// Gets the available models.
    /// </summary>
    /// <returns>The available models.</returns>
    public async Task<List<string>> GetAvailableModelsAsync()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("MCP client is not connected");
        }

        // TODO: Implement actual model retrieval
        return new List<string> { "Qwen2.5-7B-Instruct-1M" };
    }

    /// <summary>
    /// Gets the metadata for a model.
    /// </summary>
    /// <param name="modelId">The model identifier.</param>
    /// <returns>The model metadata.</returns>
    public async Task<Dictionary<string, object>> GetModelMetadataAsync(string modelId)
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("MCP client is not connected");
        }

        // TODO: Implement actual metadata retrieval
        return new Dictionary<string, object>
        {
            ["ModelId"] = modelId,
            ["Version"] = "1.0.0",
            ["Capabilities"] = new[] { "ErrorAnalysis", "ContextEnrichment", "RemediationPlanning" }
        };
    }

    /// <summary>
    /// Gets the server statistics.
    /// </summary>
    /// <returns>The server statistics.</returns>
    public async Task<Dictionary<string, object>> GetServerStatisticsAsync()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("MCP client is not connected");
        }

        // TODO: Implement actual server statistics
        return new Dictionary<string, object>
        {
            ["ConnectedClients"] = 1,
            ["ActiveContexts"] = _contextCache.Count,
            ["TotalAnalyses"] = _contextHistory.Values.Sum(h => h.Count)
        };
    }

    /// <summary>
    /// Gets the client statistics.
    /// </summary>
    /// <returns>The client statistics.</returns>
    public async Task<Dictionary<string, object>> GetClientStatisticsAsync()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("MCP client is not connected");
        }

        return new Dictionary<string, object>
        {
            ["ClientId"] = _clientId,
            ["ConnectedSince"] = _connectionStatus.LastConnected,
            ["ActiveSubscriptions"] = _contextSubscribers.Count,
            ["CachedContexts"] = _contextCache.Count,
            ["TotalHistoryEntries"] = _contextHistory.Values.Sum(h => h.Count)
        };
    }

    /// <summary>
    /// Publishes a context update.
    /// </summary>
    /// <param name="context">The context to publish.</param>
    public async Task PublishContextAsync(ErrorContext context)
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("MCP client is not connected");
        }

        // Cache the context
        _contextCache[context.ErrorId] = context;

        // Record context history
        var history = _contextHistory.GetOrAdd(context.ErrorId, _ => new List<ContextHistory>());
        history.Add(new ContextHistory
        {
            Id = Guid.NewGuid().ToString(),
            ContextId = context.ErrorId,
            Timestamp = DateTime.UtcNow,
            ChangeType = ContextChangeType.Updated,
            PreviousState = null,
            NewState = context,
            ChangedBy = _clientId,
            Metadata = new Dictionary<string, object>()
        });

        // Notify subscribers
        if (_contextSubscribers.TryGetValue(context.ErrorId, out var callback))
        {
            await callback(context);
        }
    }
} 
