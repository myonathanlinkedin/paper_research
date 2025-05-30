using System;

namespace RuntimeErrorSage.Domain.Models.Error
{
    /// <summary>
    /// Interface for creating error objects.
    /// </summary>
    public interface IErrorFactory
    {
        /// <summary>
        /// Creates a new error.
        /// </summary>
        /// <param name="type">The error type.</param>
        /// <param name="message">The error message.</param>
        /// <param name="source">The error source.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns>A new error instance.</returns>
        RuntimeError CreateError(string type, string message, string source, string stackTrace);
        
        /// <summary>
        /// Creates a new error from an exception.
        /// </summary>
        /// <param name="exception">The exception to create the error from.</param>
        /// <returns>A new error instance.</returns>
        RuntimeError FromException(Exception exception);
    }
} 
