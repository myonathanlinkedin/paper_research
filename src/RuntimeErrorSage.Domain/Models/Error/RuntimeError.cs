using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Error
{
    /// <summary>
    /// Represents a runtime error in the system.
    /// </summary>
    public class RuntimeError
    {
        /// <summary>
        /// Gets or sets the unique identifier for this error.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public string ErrorCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error severity.
        /// </summary>
        public string Severity { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the error occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the source of the error.
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the target of the error.
        /// </summary>
        public string Target { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        public string? Context { get; set; }

        /// <summary>
        /// Gets or sets the error category.
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error subcategory.
        /// </summary>
        public string? Subcategory { get; set; }

        /// <summary>
        /// Gets or sets whether the error is handled.
        /// </summary>
        public bool IsHandled { get; set; }

        /// <summary>
        /// Gets or sets whether the error is critical.
        /// </summary>
        public bool IsCritical { get; set; }

        /// <summary>
        /// Gets or sets the error correlation ID.
        /// </summary>
        public string? CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets additional metadata about the error.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the component ID associated with this error.
        /// </summary>
        public string ComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeError"/> class.
        /// </summary>
        public RuntimeError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeError"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errorType">The error type.</param>
        /// <param name="source">The error source.</param>
        /// <param name="stackTrace">The stack trace.</param>
        public RuntimeError(
            string message,
            string errorType,
            string source,
            string stackTrace)
        {
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(errorType);
            ArgumentNullException.ThrowIfNull(source);

            Message = message;
            ErrorType = errorType;
            Source = source;
            StackTrace = stackTrace;
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
            string? stackTrace = null,
            Dictionary<string, string>? metadata = null)
        {
            ArgumentNullException.ThrowIfNull(type);
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(source);

            ErrorType = type;
            Message = message;
            Source = source;
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

        /// <summary>
        /// Adds metadata to the error.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <param name="value">The metadata value.</param>
        public void AddMetadata(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            Metadata[key] = value;
        }

        /// <summary>
        /// Gets metadata from the error.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <returns>The metadata value.</returns>
        public string? GetMetadata(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return Metadata.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Validates the error.
        /// </summary>
        /// <returns>True if the error is valid; otherwise, false.</returns>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(Id))
                return false;

            if (string.IsNullOrEmpty(Message))
                return false;

            if (string.IsNullOrEmpty(ErrorType))
                return false;

            if (string.IsNullOrEmpty(Source))
                return false;

            return true;
        }

        /// <summary>
        /// Converts the error to a dictionary.
        /// </summary>
        /// <returns>The dictionary representation of the error.</returns>
        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "Id", Id },
                { "Message", Message },
                { "ErrorType", ErrorType },
                { "Source", Source },
                { "Timestamp", Timestamp },
                { "StackTrace", StackTrace },
                { "Category", Category },
                { "Severity", Severity },
                { "Target", Target },
                { "Context", Context },
                { "IsHandled", IsHandled },
                { "IsCritical", IsCritical },
                { "CorrelationId", CorrelationId },
                { "Metadata", Metadata }
            };
        }
    }
} 
