using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Context information for an error.
    /// </summary>
    public class ErrorContext
    {
        private readonly Dictionary<string, object> _metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorContext"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="timestamp">The timestamp.</param>
        public ErrorContext(
            Error error,
            string environment = null,
            DateTime? timestamp = null)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            Error = error;
            Environment = environment;
            Timestamp = timestamp ?? DateTime.UtcNow;
            _metadata = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Error Error { get; }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        public string Environment { get; }

        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata => _metadata;

        /// <summary>
        /// Adds metadata.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddMetadata(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            _metadata[key] = value;
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The metadata value.</returns>
        public object GetMetadata(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            return _metadata.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The metadata value.</returns>
        public T GetMetadata<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            if (!_metadata.TryGetValue(key, out var value))
                return default;

            return value is T typedValue ? typedValue : default;
        }

        /// <summary>
        /// Validates the context.
        /// </summary>
        /// <returns>True if the context is valid; otherwise, false.</returns>
        public bool Validate()
        {
            if (Error == null)
                return false;

            if (!Error.Validate())
                return false;

            return true;
        }

        /// <summary>
        /// Gets or sets the unique identifier for this error context.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the service where the error occurred.
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the error.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the stack trace of the error.
        /// </summary>
        public string StackTrace { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source of the error.
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the target method where the error occurred.
        /// </summary>
        public string TargetMethod { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the severity level of the error.
        /// </summary>
        public SeverityLevel Severity { get; set; }

        /// <summary>
        /// Gets or sets the system state at the time of error.
        /// </summary>
        public Dictionary<string, object> SystemState { get; set; } = new();

        /// <summary>
        /// Gets or sets additional context data.
        /// </summary>
        public Dictionary<string, object> AdditionalData { get; set; } = new();

        /// <summary>
        /// Gets or sets related errors.
        /// </summary>
        public List<ErrorContext> RelatedErrors { get; set; } = new();

        /// <summary>
        /// Gets or sets the error severity.
        /// </summary>
        public ErrorSeverity SeverityLevel { get; set; }

        /// <summary>
        /// Gets or sets the error category.
        /// </summary>
        public ErrorCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the error tags.
        /// </summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// Gets or sets the operation name.
        /// </summary>
        public string OperationName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation ID.
        /// </summary>
        public string OperationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the parent operation ID.
        /// </summary>
        public string ParentOperationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation start time.
        /// </summary>
        public DateTime OperationStartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the operation duration.
        /// </summary>
        public TimeSpan OperationDuration { get; set; }

        /// <summary>
        /// Gets or sets the operation status.
        /// </summary>
        public string OperationStatus { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation type.
        /// </summary>
        public string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation version.
        /// </summary>
        public string OperationVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation result.
        /// </summary>
        public string OperationResult { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation target.
        /// </summary>
        public string OperationTarget { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation metrics.
        /// </summary>
        public Dictionary<string, double> OperationMetrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the operation dependencies.
        /// </summary>
        public List<string> OperationDependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the operation tags.
        /// </summary>
        public Dictionary<string, string> OperationTags { get; set; } = new();

        /// <summary>
        /// Converts the error context to a dictionary.
        /// </summary>
        /// <returns>A dictionary containing the error context properties.</returns>
        public Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>
            {
                { nameof(ErrorId), ErrorId },
                { nameof(Message), Message },
                { nameof(ErrorType), ErrorType },
                { nameof(ServiceName), ServiceName },
                { nameof(Timestamp), Timestamp },
                { nameof(StackTrace), StackTrace },
                { nameof(Source), Source },
                { nameof(Severity), Severity },
                { nameof(Category), Category },
                { nameof(Tags), Tags },
                { nameof(Details), Details },
                { nameof(Metadata), Metadata },
                { nameof(OperationName), OperationName },
                { nameof(CorrelationId), CorrelationId },
                { nameof(OperationId), OperationId },
                { nameof(ParentOperationId), ParentOperationId },
                { nameof(OperationStartTime), OperationStartTime },
                { nameof(OperationDuration), OperationDuration },
                { nameof(OperationStatus), OperationStatus },
                { nameof(OperationType), OperationType },
                { nameof(OperationVersion), OperationVersion },
                { nameof(OperationResult), OperationResult },
                { nameof(OperationTarget), OperationTarget },
                { nameof(OperationMetrics), OperationMetrics },
                { nameof(OperationDependencies), OperationDependencies },
                { nameof(OperationTags), OperationTags }
            };

            return dict;
        }

        /// <summary>
        /// Explicit conversion operator from ErrorContext to Dictionary<string, object>.
        /// </summary>
        /// <param name="context">The error context to convert.</param>
        public static explicit operator Dictionary<string, object>(ErrorContext context)
        {
            return context.ToDictionary();
        }
    }
} 
