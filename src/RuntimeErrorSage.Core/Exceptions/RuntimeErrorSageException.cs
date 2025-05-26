using System;

namespace RuntimeErrorSage.Core.Exceptions;

/// <summary>
/// Custom exception type for RuntimeErrorSage-specific errors.
/// </summary>
public class RuntimeErrorSageException : Exception
{
    public RuntimeErrorSageException(string message) : base(message)
    {
    }

    public RuntimeErrorSageException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

public class RuntimeErrorSageValidationException : RuntimeErrorSageException
{
    public RuntimeErrorSageValidationException(string message) : base(message)
    {
    }

    public RuntimeErrorSageValidationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public class RuntimeErrorSageRemediationException : RuntimeErrorSageException
{
    public RuntimeErrorSageRemediationException(string message) : base(message)
    {
    }

    public RuntimeErrorSageRemediationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public class RuntimeErrorSageLLMException : RuntimeErrorSageException
{
    public RuntimeErrorSageLLMException(string message) : base(message)
    {
    }

    public RuntimeErrorSageLLMException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public class RuntimeErrorSageGraphAnalysisException : RuntimeErrorSageException
{
    public RuntimeErrorSageGraphAnalysisException(string message) : base(message)
    {
    }

    public RuntimeErrorSageGraphAnalysisException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
} 
