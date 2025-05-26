using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when there is an error publishing context to the MCP.
    /// </summary>
    public class MCPPublishingException : Exception
    {
        public MCPPublishingException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Exception thrown when there is an error retrieving data from the MCP.
    /// </summary>
    public class MCPRetrievalException : Exception
    {
        public MCPRetrievalException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Exception thrown when there is an error with MCP subscriptions.
    /// </summary>
    public class MCPSubscriptionException : Exception
    {
        public MCPSubscriptionException(string message, Exception inner) : base(message, inner) { }
    }
} 
