using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class ErrorAnalysisException : Exception
    {
        public ErrorAnalysisException(string message, Exception inner) : base(message, inner) { }
        public ErrorAnalysisException()
        {
        }

        public ErrorAnalysisException(string message) : base(message)
        {
        }
    }
} 
