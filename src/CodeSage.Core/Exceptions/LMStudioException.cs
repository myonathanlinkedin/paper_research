using System;

namespace CodeSage.Core.Exceptions
{
    public class LMStudioException : Exception
    {
        public LMStudioException(string message) : base(message) { }
        public LMStudioException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
} 