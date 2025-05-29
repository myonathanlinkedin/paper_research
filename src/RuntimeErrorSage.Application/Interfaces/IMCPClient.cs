using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.MCP;
using RuntimeErrorSage.Application.Models.Remediation;
using ContextHistory = RuntimeErrorSage.Application.Models.Context.ContextHistory;
using TimeRange = RuntimeErrorSage.Application.Models.Common.TimeRange;

namespace RuntimeErrorSage.Application.MCP.Interfaces
{
    /// <summary>
    /// Defines the interface for the Model Context Protocol client.
    /// </summary>
    public interface IMCPClient
    {
        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Gets whether the client is connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Gets the current connection status.
        /// </summary>
        MCPConnectionStatus ConnectionStatus { get; }

        /// <summary>
        /// Connects to the MCP server.
        /// </summary>
        Task ConnectAsync();

        /// <summary>
        /// Analyzes an error context.
        /// </summary>
        Task<ErrorContext> AnalyzeErrorAsync(ErrorContext context);

        /// <summary>
        /// Gets the context history for a given time range.
        /// </summary>
        Task<List<ContextHistory>> GetContextHistoryAsync(string contextId, TimeRange range);

        /// <summary>
        /// Gets the connection status for a client.
        /// </summary>
        Task<MCPConnectionStatus> GetConnectionStatusAsync(string clientId);

        /// <summary>
        /// Subscribes to context updates.
        /// </summary>
        Task SubscribeToContextUpdatesAsync(string contextId, Func<ErrorContext, Task> callback);

        /// <summary>
        /// Unsubscribes from context updates.
        /// </summary>
        Task UnsubscribeFromContextUpdatesAsync(string contextId);

        /// <summary>
        /// Gets the available models.
        /// </summary>
        Task<List<string>> GetAvailableModelsAsync();

        /// <summary>
        /// Gets metadata for a model.
        /// </summary>
        Task<Dictionary<string, object>> GetModelMetadataAsync(string modelId);

        /// <summary>
        /// Gets server statistics.
        /// </summary>
        Task<Dictionary<string, object>> GetServerStatisticsAsync();

        /// <summary>
        /// Gets client statistics.
        /// </summary>
        Task<Dictionary<string, object>> GetClientStatisticsAsync();

        /// <summary>
        /// Publishes a context update.
        /// </summary>
        Task PublishContextAsync(ErrorContext context);

        /// <summary>
        /// Updates error patterns.
        /// </summary>
        /// <param name="patterns">The patterns to update.</param>
        Task UpdateErrorPatternsAsync(List<ErrorPattern> patterns);

        /// <summary>
        /// Gets error patterns for a service.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <returns>The list of error patterns.</returns>
        Task<List<ErrorPattern>> GetErrorPatternsAsync(string serviceName);
    }
} 

