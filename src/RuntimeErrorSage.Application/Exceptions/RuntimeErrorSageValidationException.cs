using System;

namespace RuntimeErrorSage.Application.Runtime.Exceptions
{
    public class RuntimeErrorSageValidationException : RuntimeErrorSageException
    {
        public RuntimeErrorSageValidationException(string message) : base(message) { }
        public RuntimeErrorSageValidationException(string message, Exception innerException) : base(message, innerException) { }
        public RuntimeErrorSageValidationException() { }
    }
} 
