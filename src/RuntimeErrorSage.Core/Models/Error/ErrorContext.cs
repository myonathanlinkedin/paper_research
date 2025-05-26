using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Represents the context of an error occurrence.
    /// </summary>
    public class ErrorContext
    {
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
        /// Gets or sets the timestamp when the error occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets additional context information about the error.
        /// </summary>
        public Dictionary<string, object> AdditionalContext { get; set; } = new();

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
        /// Gets or sets additional properties.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; } = new();

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        public Dictionary<string, string> Environment { get; set; } = new();

        /// <summary>
        /// Gets or sets the error severity.
        /// </summary>
        public ErrorSeverity SeverityLevel { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for this error.
        /// </summary>
        public string ErrorId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets additional error details.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();

        /// <summary>
        /// Gets or sets the error category.
        /// </summary>
        public ErrorCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the error tags.
        /// </summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>
        /// Gets or sets error metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

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

    /// <summary>
    /// Defines the validation status of error analysis.
    /// </summary>
    public enum AnalysisValidationStatus
    {
        Validated,
        Pending,
        Failed,
        Skipped,
        Unknown
    }
} 
