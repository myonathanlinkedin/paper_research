using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class RuntimeErrorSageValidationException : Exception
    {
        public RuntimeErrorSageValidationException() : base() { }
        public RuntimeErrorSageValidationException(string message) : base(message) { }
        public RuntimeErrorSageValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
} 