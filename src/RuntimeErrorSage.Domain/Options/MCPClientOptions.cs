using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Options
{
    /// <summary>
    /// Configuration options for the MCP (Model Context Protocol) client.
    /// </summary>
    public class MCPClientOptions
    {
        /// <summary>
        /// Gets or sets whether cross-service communication is enabled.
        /// </summary>
        public bool EnableCrossServiceCommunication { get; } = true;

        /// <summary>
        /// Gets or sets the timeout for cross-service communication.
        /// </summary>
        public TimeSpan CrossServiceTimeout { get; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Gets or sets the maximum number of retry attempts for failed operations.
        /// </summary>
        public int MaxRetryAttempts { get; } = 3;

        /// <summary>
        /// Gets or sets the delay between retry attempts.
        /// </summary>
        public TimeSpan RetryDelay { get; } = TimeSpan.FromSeconds(1);
    }
} 







