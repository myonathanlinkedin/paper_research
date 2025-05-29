using System;

namespace RuntimeErrorSage.Application.LMStudio.Exceptions
{
    public class LMStudioException : Exception
    {
        public LMStudioException(string message) : base(message) { }
        public LMStudioException(string message, Exception innerException) 
            : base(message, innerException) { }
        public LMStudioException()
        {
        }
    }
} 

