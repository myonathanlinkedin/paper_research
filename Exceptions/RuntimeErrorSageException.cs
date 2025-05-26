using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class RuntimeErrorSageException : Exception
    {
        public RuntimeErrorSageException() : base() { }
        public RuntimeErrorSageException(string message) : base(message) { }
        public RuntimeErrorSageException(string message, Exception innerException) : base(message, innerException) { }
    }
} 