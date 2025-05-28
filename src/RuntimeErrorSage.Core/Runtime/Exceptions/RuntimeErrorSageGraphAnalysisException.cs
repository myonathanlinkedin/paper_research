using System;

namespace RuntimeErrorSage.Core.Runtime.Exceptions
{
    public class RuntimeErrorSageGraphAnalysisException : RuntimeErrorSageException
    {
        public RuntimeErrorSageGraphAnalysisException(string message) : base(message) { }
        public RuntimeErrorSageGraphAnalysisException(string message, Exception innerException) : base(message, innerException) { }
        public RuntimeErrorSageGraphAnalysisException() { }
    }
} 
