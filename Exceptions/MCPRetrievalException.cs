using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class MCPRetrievalException : Exception
    {
        public MCPRetrievalException() : base() { }
        public MCPRetrievalException(string message) : base(message) { }
        public MCPRetrievalException(string message, Exception innerException) : base(message, innerException) { }
    }
} 