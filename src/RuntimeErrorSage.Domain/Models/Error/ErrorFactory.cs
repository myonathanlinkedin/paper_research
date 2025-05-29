using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Error
{
    /// <summary>
    /// Factory for creating error objects.
    /// </summary>
    public static class ErrorFactory
    {
        /// <summary>
        /// Creates a new error.
        /// </summary>
        /// <param name="type">The error type.</param>
        /// <param name="message">The error message.</param>
        /// <param name="source">The error source.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns>A new error instance.</returns>
        public static RuntimeError Create(string type, string message, string source, string stackTrace)
        {
            return new RuntimeError
            {
                ErrorType = type,
                Message = message,
                //Source = source,
                StackTrace = stackTrace,
                Id = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creates a new error from an exception.
        /// </summary>
        /// <param name="exception">The exception to create the error from.</param>
        /// <returns>A new error instance.</returns>
        public static RuntimeError FromException(Exception exception)
        {
            return RuntimeError.FromException(exception, string.Empty);
        }
    }
} 
