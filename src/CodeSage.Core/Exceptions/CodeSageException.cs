using System;

namespace CodeSage.Core.Exceptions
{
    /// <summary>
    /// Custom exception type for CodeSage-specific errors.
    /// </summary>
    public class CodeSageException : Exception
    {
        public CodeSageException(string message) : base(message)
        {
        }

        public CodeSageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
} 