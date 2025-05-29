using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Exceptions
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







