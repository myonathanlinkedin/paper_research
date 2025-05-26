using System;

namespace CodeSage.Core.Exceptions
{
    public class RemediationValidationException : Exception
    {
        public RemediationValidationException(string message) : base(message) { }
    }
} 