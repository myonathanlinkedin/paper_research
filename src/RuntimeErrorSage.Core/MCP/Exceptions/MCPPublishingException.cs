using System;

namespace RuntimeErrorSage.Core.MCP.Exceptions
{
    /// <summary>
    /// Exception thrown when there is an error publishing context to the MCP.
    /// </summary>
    public class MCPPublishingException : Exception
    {
        public MCPPublishingException(string message, Exception inner) : base(message, inner) { }
        public MCPPublishingException(string message) : base(message) { }
        public MCPPublishingException()
        {
        }
    }
} 
