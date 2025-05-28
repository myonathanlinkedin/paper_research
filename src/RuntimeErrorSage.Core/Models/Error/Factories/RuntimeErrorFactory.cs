using System;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Models.Error.Factories
{
    /// <summary>
    /// Factory for creating RuntimeError instances.
    /// </summary>
    public class RuntimeErrorFactory : IRuntimeErrorFactory
    {
        /// <summary>
        /// Creates a new RuntimeError instance.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errorType">The error type.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns>A new RuntimeError instance.</returns>
        public RuntimeError Create(string message, string errorType = null, string stackTrace = null)
        {
            return new RuntimeError
            {
                Id = Guid.NewGuid().ToString(),
                Message = message,
                ErrorType = errorType ?? "System.Exception",
                StackTrace = stackTrace,
                Timestamp = DateTime.UtcNow
            };
        }
    }
} 