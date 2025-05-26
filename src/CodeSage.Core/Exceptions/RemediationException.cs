using System;

namespace CodeSage.Core.Exceptions
{
    public class RemediationException : Exception
    {
        public RemediationException(string message, Exception inner) : base(message, inner) { }
    }
} 