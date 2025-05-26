using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class LMStudioException : Exception
    {
        public LMStudioException(string message) : base(message) { }
        public LMStudioException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
} 
