using System.Threading.Tasks;

namespace CodeSage.Core
{
    /// <summary>
    /// Defines the interface for the Model Context Protocol client.
    /// </summary>
    public interface IMCPClient
    {
        /// <summary>
        /// Publishes error context and analysis to the MCP network.
        /// </summary>
        /// <param name="context">The error context to publish</param>
        /// <param name="analysis">The analysis result to publish</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task PublishContextAsync(ErrorContext context, ErrorAnalysisResult analysis);

        /// <summary>
        /// Subscribes to error context updates from the MCP network.
        /// </summary>
        /// <param name="handler">The handler to process incoming context updates</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task SubscribeToContextAsync(Func<ErrorContext, ErrorAnalysisResult, Task> handler);

        /// <summary>
        /// Retrieves historical context data for analysis.
        /// </summary>
        /// <param name="correlationId">The correlation ID to search for</param>
        /// <param name="timeRange">The time range to search within</param>
        /// <returns>A collection of historical context data</returns>
        Task<IEnumerable<ContextHistory>> GetContextHistoryAsync(
            string correlationId,
            TimeRange timeRange);

        /// <summary>
        /// Initializes the MCP client with the specified endpoint.
        /// </summary>
        /// <param name="endpoint">The MCP endpoint to connect to</param>
        /// <returns>A task representing the asynchronous initialization</returns>
        Task InitializeAsync(string endpoint);

        /// <summary>
        /// Gets the current connection status of the MCP client.
        /// </summary>
        /// <returns>The current connection status</returns>
        Task<ConnectionStatus> GetConnectionStatusAsync();
    }

    /// <summary>
    /// Represents a time range for context history queries.
    /// </summary>
    public class TimeRange
    {
        /// <summary>
        /// Gets or sets the start time of the range.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the range.
        /// </summary>
        public DateTime EndTime { get; set; }
    }

    /// <summary>
    /// Represents historical context data.
    /// </summary>
    public class ContextHistory
    {
        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        public ErrorContext Context { get; set; } = new();

        /// <summary>
        /// Gets or sets the analysis result.
        /// </summary>
        public ErrorAnalysisResult Analysis { get; set; } = new();

        /// <summary>
        /// Gets or sets the timestamp when the context was recorded.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the source service that generated the context.
        /// </summary>
        public string SourceService { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the connection status of the MCP client.
    /// </summary>
    public class ConnectionStatus
    {
        /// <summary>
        /// Gets or sets whether the client is connected to the MCP network.
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// Gets or sets the endpoint the client is connected to.
        /// </summary>
        public string Endpoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last time the connection was established.
        /// </summary>
        public DateTime LastConnected { get; set; }

        /// <summary>
        /// Gets or sets the number of active subscriptions.
        /// </summary>
        public int ActiveSubscriptions { get; set; }

        /// <summary>
        /// Gets or sets the number of messages published.
        /// </summary>
        public long MessagesPublished { get; set; }

        /// <summary>
        /// Gets or sets the number of messages received.
        /// </summary>
        public long MessagesReceived { get; set; }
    }
} 