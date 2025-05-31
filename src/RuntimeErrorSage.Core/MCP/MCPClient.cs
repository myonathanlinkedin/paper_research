using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Exceptions;
using RuntimeErrorSage.Application.Extensions;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Domain.Models.Common;
using RuntimeErrorSage.Domain.Models.Context;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Application.Storage.Interfaces;
using ConnectionState = RuntimeErrorSage.Domain.Enums.ConnectionState;
using ContextHistory = RuntimeErrorSage.Domain.Models.Context.ContextHistory;
using TimeRange = RuntimeErrorSage.Domain.Models.Common.TimeRange;
using RuntimeErrorSage.Domain.Models.MCP;
using RuntimeErrorSage.Domain.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.RegularExpressions;

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
    private readonly IPatternStorage _storage;
    private readonly IErrorAnalyzer _errorAnalyzer;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the MCPClient class.
    /// </summary>
    public MCPClient(
        ILogger<MCPClient> logger,
        IOptions<MCPClientOptions> options,
        IPatternStorage storage,
        IErrorAnalyzer errorAnalyzer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _errorAnalyzer = errorAnalyzer ?? throw new ArgumentNullException(nameof(errorAnalyzer));
        
        _clientId = Guid.NewGuid().ToString();
        _connectionStatus = new MCPConnectionStatus
        {
            State = ConnectionState.Disconnected,
            LastAttempt = DateTime.UtcNow
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
            bool isConnectionValid = await ValidateConnectionAsync();
            if (!isConnectionValid)
            {
                throw new MCPException("Failed to connect to pattern storage");
            }

            // Initialize context cache
            await InitializeContextCacheAsync();

            _connectionStatus.State = ConnectionState.Connected;
            _connectionStatus.LastSuccess = DateTime.UtcNow;
            _connectionStatus.LastConnected = DateTime.UtcNow;
            _connectionStatus.LastError = null;
            _logger.LogInformation("MCP client connected with ID {ClientId}", _clientId);
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

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        try
        {
            // Analyze the error
            var analysisResult = await _errorAnalyzer.AnalyzeAsync(context.Error);

            // Use the AddMetadata method to add the analysis result
            context.AddMetadata("LatestAnalysisResult", analysisResult);
            
            // Cache the context
            _contextCache[context.ErrorId] = context;

            // Update context history
            await UpdateContextHistoryAsync(context);

            return context;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing context {ContextId}", context.ErrorId);
            throw new MCPException("Failed to analyze error context", ex);
        }
    }

    /// <summary>
    /// Gets the complete context history.
    /// </summary>
    /// <param name="contextId">The context identifier.</param>
    /// <param name="range">The time range to get history for.</param>
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
            .Where(h => h.Timestamp >= range.StartTime && h.Timestamp <= range.EndTime)
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
        var history = _contextHistory.TryGetValue(context.ErrorId, out var existingHistory) 
            ? existingHistory 
            : new List<ContextHistory>();
            
        if (!_contextHistory.ContainsKey(context.ErrorId))
        {
            _contextHistory[context.ErrorId] = history;
        }
        
        history.Add(new ContextHistory
        {
            Id = Guid.NewGuid().ToString(),
            ContextId = context.ErrorId,
            Timestamp = DateTime.UtcNow,
            ChangeType = "Analyzed",
            PreviousState = null,
            NewState = null, // We can't directly cast ErrorContext to Dictionary<string, object>
            ChangedBy = _clientId,
            Metadata = new Dictionary<string, object>()
        });

        // Notify subscribers
        if (_contextSubscribers.TryGetValue(context.ErrorId, out var callback))
        {
            await callback(context);
        }
    }

    private async Task<bool> ValidateConnectionAsync()
    {
        try
        {
            // Check if we can access the storage
            if (_storage == null)
            {
                throw new MCPException("Pattern storage is not available");
            }
            
            // This would be where we'd check connectivity to the storage
            // This is just a placeholder as ValidateConnectionAsync doesn't exist on IPatternStorage
            await Task.Delay(10);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate connection");
            throw new MCPException("Failed to validate connection to storage", ex);
        }
    }

    private async Task InitializeContextCacheAsync()
    {
        try
        {
            // Fix: Add the required pattern parameter to GetPatternsAsync
            var patterns = await _storage.GetPatternsAsync("error:*");
            foreach (var pattern in patterns)
            {
                // Fix: Parse the string value into an ErrorContext
                _contextCache[pattern.Key] = ParseErrorContext(pattern.Value);
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
        if (!_contextHistory.TryGetValue(context.ErrorId, out var history))
        {
            history = new List<ContextHistory>();
            _contextHistory[context.ErrorId] = history;
        }

        history.Add(new ContextHistory
        {
            Id = Guid.NewGuid().ToString(),
            ContextId = context.ErrorId,
            Timestamp = DateTime.UtcNow,
            ChangeType = "Analyzed",
            PreviousState = null,
            NewState = null,
            ChangedBy = _clientId,
            Metadata = new Dictionary<string, object>
            {
                ["AnalysisTimestamp"] = DateTime.UtcNow
            }
        });
    }

    private async Task NotifySubscribersAsync(ErrorContext context)
    {
        try
        {
            if (_contextSubscribers.TryGetValue(context.ErrorId, out var callback))
            {
                await callback(context);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notifying subscribers for context {ContextId}", context.ErrorId);
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
        
        // Convert List<ErrorPattern> to Dictionary<string, string> if SavePatternsAsync requires it
        var patternsDict = patterns.ToDictionary(p => p.Id, p => JsonSerializer.Serialize(p));
        await _storage.SavePatternsAsync(patternsDict);
    }

    public async Task<List<ErrorPattern>> GetErrorPatternsAsync(string serviceName)
    {
        // Get patterns as dictionary
        var patternsDict = await _storage.GetPatternsAsync("*");
        
        // Convert dictionary to list of ErrorPattern objects
        var allPatterns = new List<ErrorPattern>();
        foreach (var kvp in patternsDict)
        {
            try
            {
                var pattern = new ErrorPattern
                {
                    Id = kvp.Key,
                    ServiceName = ExtractServiceName(kvp.Value),
                    Pattern = kvp.Value
                };
                allPatterns.Add(pattern);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse pattern {PatternId}", kvp.Key);
            }
        }
        
        // Filter by service name if provided
        if (string.IsNullOrEmpty(serviceName))
            return allPatterns;
            
        return allPatterns.Where(p => p.ServiceName == serviceName).ToList();
    }
    
    // Helper method to extract service name from pattern string
    private string ExtractServiceName(string pattern)
    {
        // Simple implementation - in a real system, this would parse the pattern
        if (string.IsNullOrEmpty(pattern))
            return string.Empty;
            
        // Look for a service name pattern like "service:name" in the string
        var match = Regex.Match(pattern, @"service:([^\s;]+)");
        return match.Success ? match.Groups[1].Value : "unknown";
    }

    // Helper method to convert a string to an ErrorContext
    private ErrorContext ParseErrorContext(string context)
    {
        try
        {
            if (string.IsNullOrEmpty(context))
                return null;
                
            // Try to deserialize the context if it's JSON
            if (context.StartsWith("{") && context.EndsWith("}"))
            {
                try
                {
                    return JsonSerializer.Deserialize<ErrorContext>(context, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogWarning(jsonEx, "Failed to deserialize error context as JSON");
                }
            }
                
            // If not JSON or JSON parsing failed, create a basic ErrorContext with minimal info
            var errorParts = context.Split('|');
            string errorType = errorParts.Length > 0 ? errorParts[0] : "UnknownErrorType";
            string message = errorParts.Length > 1 ? errorParts[1] : context;
            string source = errorParts.Length > 2 ? errorParts[2] : "MCPClient";
            
            return new ErrorContext(
                new RuntimeError(
                    message,
                    errorType,
                    source,
                    null),
                "MCPClient",
                DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse error context string");
            return null;
        }
    }
    
    // Helper method to convert an ErrorContext to a string
    private string SerializeErrorContext(ErrorContext context)
    {
        if (context == null)
            return null;
            
        try
        {
            // Serialize to JSON
            return JsonSerializer.Serialize(context, new JsonSerializerOptions
            {
                WriteIndented = false
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to serialize error context");
            
            // Fallback to simple string format
            return $"{context.ErrorType}|{context.Message}|{context.Source}";
        }
    }
} 

