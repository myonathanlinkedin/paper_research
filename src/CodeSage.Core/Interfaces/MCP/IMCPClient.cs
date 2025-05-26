using System.Collections.Generic;
using System.Threading.Tasks;
using CodeSage.Core.Models.Error;
using CodeSage.Core.Models.MCP;
using CodeSage.Core.Models;

namespace CodeSage.Core.Interfaces.MCP
{
    /// <summary>
    /// Defines the interface for the Machine Control Protocol (MCP) client.
    /// </summary>
    public interface IMCPClient
    {
        /// <summary>
        /// Gets the current connection status.
        /// </summary>
        Task<MCPConnectionStatus> GetConnectionStatusAsync();

        /// <summary>
        /// Publishes an error context and optional analysis to the MCP server.
        /// </summary>
        Task PublishContextAsync(ErrorContext context, ErrorAnalysisResult? analysis = null);

        /// <summary>
        /// Gets error patterns for a service.
        /// </summary>
        Task<List<ErrorPattern>> GetErrorPatternsAsync(string serviceName);

        /// <summary>
        /// Updates error patterns for a service.
        /// </summary>
        Task UpdateErrorPatternsAsync(string serviceName, List<ErrorPattern> patterns);

        /// <summary>
        /// Deletes error patterns for a service.
        /// </summary>
        Task DeleteErrorPatternsAsync(string serviceName, List<string> patternIds);

        /// <summary>
        /// Checks if the client is connected to the MCP server.
        /// </summary>
        Task<bool> IsConnectedAsync();

        /// <summary>
        /// Disconnects from the MCP server.
        /// </summary>
        Task DisconnectAsync();

        /// <summary>
        /// Attempts to reconnect to the MCP server.
        /// </summary>
        Task ReconnectAsync();
    }
} 