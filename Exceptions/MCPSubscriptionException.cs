using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class MCPSubscriptionException : Exception
    {
        public MCPSubscriptionException() : base() { }
        public MCPSubscriptionException(string message) : base(message) { }
        public MCPSubscriptionException(string message, Exception innerException) : base(message, innerException) { }
    }
} 