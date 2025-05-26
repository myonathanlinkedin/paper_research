using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class RemediationValidationException : Exception
    {
        public RemediationValidationException(string message) : base(message) { }
        public RemediationValidationException()
        {
        }

        public RemediationValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 
