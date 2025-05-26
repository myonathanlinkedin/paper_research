using System;

namespace CodeSage.Core.Exceptions
{
    public class ErrorAnalysisException : Exception
    {
        public ErrorAnalysisException(string message, Exception inner) : base(message, inner) { }
    }
} 