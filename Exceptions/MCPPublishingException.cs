using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class MCPPublishingException : Exception
    {
        public MCPPublishingException() : base() { }
        public MCPPublishingException(string message) : base(message) { }
        public MCPPublishingException(string message, Exception innerException) : base(message, innerException) { }
    }
} 