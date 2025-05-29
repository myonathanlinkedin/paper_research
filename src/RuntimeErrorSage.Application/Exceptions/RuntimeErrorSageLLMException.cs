using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Runtime.Exceptions
{
    public class RuntimeErrorSageLLMException : RuntimeErrorSageException
    {
        public RuntimeErrorSageLLMException(string message) : base(message) { }
        public RuntimeErrorSageLLMException(string message, Exception innerException) : base(message, innerException) { }
        public RuntimeErrorSageLLMException() { }
    }
} 






