using System;

namespace RuntimeErrorSage.Application.Runtime.Exceptions
{
    public class RuntimeErrorSageRemediationException : RuntimeErrorSageException
    {
        public RuntimeErrorSageRemediationException(string message) : base(message) { }
        public RuntimeErrorSageRemediationException(string message, Exception innerException) : base(message, innerException) { }
        public RuntimeErrorSageRemediationException() { }
    }
} 
