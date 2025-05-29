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
        public bool EnableCrossServiceCommunication { get; set; } = true;

        /// <summary>
        /// Gets or sets the timeout for cross-service communication.
        /// </summary>
        public TimeSpan CrossServiceTimeout { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Gets or sets the maximum number of retry attempts for failed operations.
        /// </summary>
        public int MaxRetryAttempts { get; set; } = 3;

        /// <summary>
        /// Gets or sets the delay between retry attempts.
        /// </summary>
        public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(1);
    }
} 

