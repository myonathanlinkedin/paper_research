using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class RuntimeErrorSageLLMException : RuntimeErrorSageException
    {
        public RuntimeErrorSageLLMException(string message) : base(message) { }
        public RuntimeErrorSageLLMException(string message, Exception innerException) : base(message, innerException) { }
        public RuntimeErrorSageLLMException() { }
    }
} 