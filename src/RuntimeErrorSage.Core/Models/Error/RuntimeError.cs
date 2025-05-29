using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Represents a runtime error in the system.
    /// </summary>
    public class RuntimeError
    {
        /// <summary>
        /// Gets or sets the unique identifier of the error.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType { get; set; }

        /// <summary>
        /// Gets or sets the component ID where the error occurred.
        /// </summary>
        public string ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the error occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the stack trace of the error.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets additional metadata about the error.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeError"/> class.
        /// </summary>
        public RuntimeError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeError"/> class.
        /// </summary>
        /// <param name="type">The error type.</param>
        /// <param name="message">The error message.</param>
        /// <param name="source">The error source.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <param name="metadata">Additional metadata.</param>
        public RuntimeError(
            string type,
            string message,
            string source,
            string stackTrace = null,
            Dictionary<string, string> metadata = null)
        {
            ArgumentNullException.ThrowIfNull(type);
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(source);

            ErrorType = type;
            Message = message;
            StackTrace = stackTrace;
            if (metadata != null)
            {
                Metadata = metadata;
            }
        }

        /// <summary>
        /// Creates a runtime error from an exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="source">The error source.</param>
        /// <returns>A new runtime error.</returns>
        public static RuntimeError FromException(Exception exception, string source)
        {
            ArgumentNullException.ThrowIfNull(exception);
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("Source cannot be null or empty", nameof(source));

            return new RuntimeError(
                type: exception.GetType().Name,
                message: exception.Message,
                source: source,
                stackTrace: exception.StackTrace);
        }
    }
} 
