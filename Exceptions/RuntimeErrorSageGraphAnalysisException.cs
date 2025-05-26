using System;

namespace RuntimeErrorSage.Core.Exceptions
{
    public class RuntimeErrorSageGraphAnalysisException : Exception
    {
        public RuntimeErrorSageGraphAnalysisException() : base() { }
        public RuntimeErrorSageGraphAnalysisException(string message) : base(message) { }
        public RuntimeErrorSageGraphAnalysisException(string message, Exception innerException) : base(message, innerException) { }
    }
} 