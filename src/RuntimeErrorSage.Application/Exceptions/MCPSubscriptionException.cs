using System;

namespace RuntimeErrorSage.Application.MCP.Exceptions
{
    /// <summary>
    /// Exception thrown when there is an error with MCP subscriptions.
    /// </summary>
    public class MCPSubscriptionException : Exception
    {
        public MCPSubscriptionException(string message, Exception inner) : base(message, inner) { }
        public MCPSubscriptionException() { }
        public MCPSubscriptionException(string message) : base(message) { }
    }
} 
