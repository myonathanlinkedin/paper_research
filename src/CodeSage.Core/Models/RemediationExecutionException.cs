using System;

namespace CodeSage.Core.Models
{
    public class RemediationExecutionException : Exception
    {
        public RemediationExecutionException(string message, Exception inner) : base(message, inner) { }
        public RemediationExecutionException(string message) : base(message) { }
    }
} 