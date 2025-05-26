using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class RuntimeErrorSageRemediationException : RuntimeErrorSageException
    {
        public RuntimeErrorSageRemediationException(string message) : base(message) { }
        public RuntimeErrorSageRemediationException(string message, Exception innerException) : base(message, innerException) { }
        public RuntimeErrorSageRemediationException() { }
    }
} 