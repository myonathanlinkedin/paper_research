using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class RuntimeErrorSageLLMException : Exception
    {
        public RuntimeErrorSageLLMException() : base() { }
        public RuntimeErrorSageLLMException(string message) : base(message) { }
        public RuntimeErrorSageLLMException(string message, Exception innerException) : base(message, innerException) { }
    }
} 