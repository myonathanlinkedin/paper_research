using System;

namespace RuntimeErrorSage.Model.Exceptions
{
    public class RemediationException : Exception
    {
        public RemediationException(string message, Exception inner) : base(message, inner) { }
        public RemediationException()
        {
        }

        public RemediationException(string message) : base(message)
        {
        }
    }
} 

