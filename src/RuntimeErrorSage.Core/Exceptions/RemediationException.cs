using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class RemediationException : Exception
    {
        public RemediationException(string message, Exception inner) : base(message, inner) { }
    }
} 
