using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Core.Analysis.Interfaces;
using RuntimeErrorSage.Core.Exceptions;
using RuntimeErrorSage.Core.Extensions;
using RuntimeErrorSage.Core.MCP.Interfaces;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Context;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Storage.Interfaces;
using ConnectionState = RuntimeErrorSage.Core.Models.Enums.ConnectionState;
using ContextHistory = RuntimeErrorSage.Core.Models.Context.ContextHistory;
using TimeRange = RuntimeErrorSage.Core.Models.Common.TimeRange;
using RuntimeErrorSage.Core.Models.MCP;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.MCP;

/// <summary>
/// Implements the Model Context Protocol (MCP) client for error context analysis.
/// This implementation follows the specifications in the research paper.
/// </summary>
public class MCPClient : IMCPClient, IDisposable
{
    private readonly ILogger<MCPClient> _logger;
    private readonly string _clientId;
    private readonly MCPConnectionStatus _connectionStatus;
    private readonly Dictionary<string, ErrorContext> _contextCache;
    private readonly Dictionary<string, List<ContextHistory>> _contextHistory;
    private readonly Dictionary<string, Func<ErrorContext, Task>> _contextSubscribers;
    private readonly MCPClientOptions _options;
    private readonly RuntimeErrorSage.Core.Storage.Interfaces.IPatternStorage _storage;
    private readonly IErrorAnalyzer _errorAnalyzer;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the MCPClient class.
    /// </summary>
    public MCPClient(
        ILogger<MCPClient> logger,
        IOptions<MCPClientOptions> options,
        RuntimeErrorSage.Core.Storage.Interfaces.IPatternStorage storage,
        IErrorAnalyzer errorAnalyzer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _errorAnalyzer = errorAnalyzer ?? throw new ArgumentNullException(nameof(errorAnalyzer));
        
        _clientId = Guid.NewGuid().ToString();
        _connectionStatus = new MCPConnectionStatus { State = ConnectionState.Disconnected };
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
    public bool IsConnected => _connectionStatus.State == ConnectionState.Connected;

    /// <summary>
    /// Gets the current connection status.
    /// </summary>
    public MCPConnectionStatus ConnectionStatus => _connectionStatus;

    /// <summary>
    /// Connects to the MCP server.
    /// </summary>
    public async Task ConnectAsync()
    {
        if (IsConnected)
        {
            return;
        }

        try
        {
            _connectionStatus.State = ConnectionState.Connecting;
            _connectionStatus.LastAttempt = DateTime.UtcNow;

            // Validate storage connection
            if (!await _storage.ValidateConnectionAsync())
            {
                throw new MCPException("Failed to connect to pattern storage");
            }

            // Initialize context cache
            await InitializeContextCacheAsync();

            _connectionStatus.State = ConnectionState.Connected;
            _connectionStatus.LastSuccess = DateTime.UtcNow;
            _logger.LogInformation("MCP client connected successfully");
        }
        catch (Exception ex)
        {
            _connectionStatus.State = ConnectionState.Failed;
            _connectionStatus.LastError = ex.Message;
            _logger.LogError(ex, "Failed to connect MCP client");
            throw new MCPException("Failed to connect MCP client", ex);
        }
    }

    /// <summary>
    /// Analyzes an error context.
    /// </summary>
    /// <param name="context">The error context to analyze.</param>
    /// <returns>The analyzed error context.</returns>
    public async Task<ErrorContext> AnalyzeErrorAsync(ErrorContext context)
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("MCP client is not connected");
        }

        try
        {
            // Validate context
            if (!await ValidateContextAsync(context))
            {
                throw new MCPException("Invalid error context");
            }

            // Analyze error using graph-based analysis
            var analysis = await _errorAnalyzer.AnalyzeContextAsync(context);

            // Update context with analysis results
            context.AddMetadata("AnalysisResult", analysis);

            // Cache context
            _contextCache[context.ErrorId] = context;

            // Update history
            await UpdateContextHistoryAsync(context);

            // Notify subscribers
            await NotifySubscribersAsync(context);

            return context;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing context: {ErrorId}", context.ErrorId);
            throw new MCPException("Failed to analyze error context", ex);
        }
    }

    /// <summary>
    /// Gets the complete context history.
    /// </summary>
    /// <param name="contextId">The context identifier.</param>
    /// <returns>The complete context history.</returns>
    public async Task<List<ContextHistory>> GetContextHistoryAsync(string contextId, TimeRange range)
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("MCP client is not connected");
        }

        if (!_contextHistory.ContainsKey(contextId))
        {
            return new List<ContextHistory>();
        }

        return _contextHistory[contextId]
            .Where(h => h.Timestamp >= range.Start && h.Timestamp <= range.End)
            .OrderByDescending(h => h.Timestamp)
            .ToList();
    }

    /// <summary>
    /// Gets the connection status for a client.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <returns>The connection status.</returns>
    public async Task<MCPConnectionStatus> GetConnectionStatusAsync(string clientId)
    {
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
            NewState = (Dictionary<string, object>)context,
            ChangedBy = _clientId,
            Metadata = new Dictionary<string, object>()
        });

        // Notify subscribers
        if (_contextSubscribers.TryGetValue(context.ErrorId, out var callback))
        {
            await callback(context);
        }
    }

    private async Task InitializeContextCacheAsync()
    {
        try
        {
            var patterns = await _storage.GetPatternsAsync();
            foreach (var pattern in patterns)
            {
                _contextCache[pattern.Id] = pattern.Context;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize context cache");
            throw;
        }
    }

    private async Task<bool> ValidateContextAsync(ErrorContext context)
    {
        if (context == null)
        {
            return false;
        }

        // Validate required fields
        if (string.IsNullOrEmpty(context.ErrorId) ||
            string.IsNullOrEmpty(context.ErrorType) ||
            string.IsNullOrEmpty(context.Message))
        {
            return false;
        }

        // Validate component graph
        if (context.ComponentGraph == null || !context.ComponentGraph.Any())
        {
            return false;
        }

        return true;
    }

    private async Task UpdateContextHistoryAsync(ErrorContext context)
    {
        if (!_contextHistory.ContainsKey(context.ErrorId))
        {
            _contextHistory[context.ErrorId] = new List<ContextHistory>();
        }

        _contextHistory[context.ErrorId].Add(new ContextHistory
        {
            ContextId = context.ErrorId,
            Timestamp = DateTime.UtcNow,
            ServiceName = context.ServiceName,
            OperationName = context.OperationName,
            MostRecentContext = context,
            Metadata = new Dictionary<string, object>(context.Metadata)
        });
    }

    private async Task NotifySubscribersAsync(ErrorContext context)
    {
        if (_contextSubscribers.TryGetValue(context.ErrorId, out var callback))
        {
            try
            {
                await callback(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error notifying subscriber for context: {ErrorId}", context.ErrorId);
            }
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _contextCache.Clear();
            _contextHistory.Clear();
            _contextSubscribers.Clear();
            _disposed = true;
        }
    }

    public async Task UpdateErrorPatternsAsync(List<ErrorPattern> patterns)
    {
        if (patterns == null) throw new ArgumentNullException(nameof(patterns));
        await _storage.SavePatternsAsync(patterns);
    }

    public async Task<List<ErrorPattern>> GetErrorPatternsAsync(string serviceName)
    {
        var allPatterns = await _storage.GetPatternsAsync();
        if (string.IsNullOrEmpty(serviceName))
            return allPatterns;
        return allPatterns.FindAll(p => p.ServiceName == serviceName);
    }
} 

