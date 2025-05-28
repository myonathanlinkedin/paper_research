using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Represents a runtime error that occurs in the system.
    /// </summary>
    public class RuntimeError
    {
        /// <summary>
        /// Gets or sets the unique identifier of the error.
        /// </summary>
        public string ErrorId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public string ErrorCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        public string StackTrace { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the inner exception message.
        /// </summary>
        public string InnerExceptionMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source of the error.
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component ID.
        /// </summary>
        public string ComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the severity of the error.
        /// </summary>
        public SeverityLevel Severity { get; set; } = SeverityLevel.Medium;

        /// <summary>
        /// Gets or sets the timestamp when the error occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the additional context.
        /// </summary>
        public Dictionary<string, object> AdditionalContext { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets whether the error has been resolved.
        /// </summary>
        public bool IsResolved { get; set; } = false;

        /// <summary>
        /// Gets or sets the resolution description.
        /// </summary>
        public string ResolutionDescription { get; set; } = string.Empty;

        /// <summary>
        /// Creates a new RuntimeError from an exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The runtime error.</returns>
        public static RuntimeError FromException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return new RuntimeError
            {
                Message = exception.Message,
                ErrorType = exception.GetType().Name,
                StackTrace = exception.StackTrace ?? string.Empty,
                InnerExceptionMessage = exception.InnerException?.Message ?? string.Empty,
                Source = exception.Source ?? string.Empty,
                Timestamp = DateTime.UtcNow
            };
        }
    }
} 