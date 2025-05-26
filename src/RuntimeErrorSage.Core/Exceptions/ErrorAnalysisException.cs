using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class ErrorAnalysisException : Exception
    {
        public ErrorAnalysisException(string message, Exception inner) : base(message, inner) { }
    }
} 
