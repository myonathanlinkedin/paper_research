using System;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Interface for creating error instances.
    /// </summary>
    public interface IErrorFactory
    {
        /// <summary>
        /// Creates a new error instance.
        /// </summary>
        /// <param name="type">The error type.</param>
        /// <param name="message">The error message.</param>
        /// <param name="source">The error source.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <returns>A new error instance.</returns>
        Error CreateError(string type, string message, string source, string stackTrace);
    }
} 