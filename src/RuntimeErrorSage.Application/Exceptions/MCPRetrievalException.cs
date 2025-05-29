using System;

namespace RuntimeErrorSage.Application.MCP.Exceptions
{
    /// <summary>
    /// Exception thrown when there is an error retrieving data from the MCP.
    /// </summary>
    public class MCPRetrievalException : Exception
    {
        public MCPRetrievalException(string message, Exception inner) : base(message, inner) { }
        public MCPRetrievalException() { }
        public MCPRetrievalException(string message) : base(message) { }
    }
} 
