using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Represents the context of an error, including exception details and additional metadata.
    /// </summary>
    public class ErrorContext
    {
        /// <summary>
        /// Gets or sets the unique identifier for this error context.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error identifier.
        /// </summary>
        public string ErrorId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the correlation identifier for tracking related errors.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component identifier where the error occurred.
        /// </summary>
        public string ComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the exception that caused the error.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the stack trace of the error.
        /// </summary>
        public string StackTrace { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the error occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets additional context information.
        /// </summary>
        public Dictionary<string, string> AdditionalContext { get; set; } = new();

        private readonly Dictionary<string, object> _metadata = new();

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata => _metadata;

        /// <summary>
        /// Gets or sets the error source component.
        /// </summary>
        public string ErrorSource { get; set; }

        /// <summary>
        /// Gets or sets the component graph data.
        /// </summary>
        public Dictionary<string, HashSet<string>> ComponentGraph { get; set; } = new();

        /// <summary>
        /// Gets or sets the dependency graph at the time of the error.
        /// </summary>
        public DependencyGraph DependencyGraph { get; set; }

        /// <summary>
        /// Gets or sets the affected components in the dependency graph.
        /// </summary>
        public List<GraphNode> AffectedComponents { get; set; }

        /// <summary>
        /// Gets or sets the metrics collected at the time of the error.
        /// </summary>
        public Dictionary<string, double> Metrics { get; set; }

        /// <summary>
        /// Gets or sets the list of previous errors that may have contributed to this error.
        /// </summary>
        public List<ErrorContext> PreviousErrors { get; set; }

        /// <summary>
        /// Gets or sets the analysis result of the error.
        /// </summary>
        public ErrorAnalysisResult AnalysisResult { get; set; }

        /// <summary>
        /// Gets or sets the context identifier.
        /// </summary>
        public string ContextId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

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
        public ErrorSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the environment where the error occurred.
        /// </summary>
        public string Environment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source of the error.
        /// </summary>
        public string Source { get; set; } = string.Empty;

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
        /// Gets or sets the service calls related to this error context.
        /// </summary>
        public List<ServiceCall> ServiceCalls { get; set; } = new();

        /// <summary>
        /// Gets or sets the data flows related to this error context.
        /// </summary>
        public List<DataFlow> DataFlows { get; set; } = new();

        /// <summary>
        /// Gets or sets the component metrics for this error context.
        /// </summary>
        public Dictionary<string, Dictionary<string, double>> ComponentMetrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the component dependencies for this error context.
        /// </summary>
        public List<ComponentDependency> ComponentDependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets additional error context data.
        /// </summary>
        public Dictionary<string, object> ContextData { get; set; } = new();

        /// <summary>
        /// Gets or sets the inner error context if any.
        /// </summary>
        public ErrorContext InnerError { get; set; }

        /// <summary>
        /// Gets or sets the name of the service where the error occurred.
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

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
            Error = error;
            Environment = environment ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown";
            Timestamp = timestamp ?? DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Error Error { get; }

        /// <summary>
        /// Adds metadata to the error context.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <param name="value">The metadata value.</param>
        public void AddMetadata(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            _metadata[key] = value;
        }

        /// <summary>
        /// Gets metadata from the error context.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <returns>The metadata value.</returns>
        public object GetMetadata(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return _metadata.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Gets typed metadata from the error context.
        /// </summary>
        /// <typeparam name="T">The type of the metadata value.</typeparam>
        /// <param name="key">The metadata key.</param>
        /// <returns>The typed metadata value.</returns>
        public T GetMetadata<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return _metadata.TryGetValue(key, out var value) && value is T typedValue ? typedValue : default;
        }

        /// <summary>
        /// Validates the error context.
        /// </summary>
        /// <returns>True if the error context is valid, false otherwise.</returns>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(ErrorId))
                return false;

            if (string.IsNullOrEmpty(ComponentId))
                return false;

            if (Exception == null && string.IsNullOrEmpty(Message))
                return false;

            return true;
        }

        /// <summary>
        /// Converts the error context to a dictionary.
        /// </summary>
        /// <returns>A dictionary representation of the error context.</returns>
        public Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>
            {
                ["Id"] = Id,
                ["ErrorId"] = ErrorId,
                ["CorrelationId"] = CorrelationId,
                ["ComponentId"] = ComponentId,
                ["Timestamp"] = Timestamp,
                ["Environment"] = Environment,
                ["ErrorType"] = ErrorType,
                ["Severity"] = Severity,
                ["Category"] = Category,
                ["Message"] = Message,
                ["StackTrace"] = StackTrace,
                ["ServiceName"] = ServiceName,
                ["OperationName"] = OperationName,
                ["OperationId"] = OperationId,
                ["ParentOperationId"] = ParentOperationId,
                ["OperationStartTime"] = OperationStartTime,
                ["OperationDuration"] = OperationDuration,
                ["OperationStatus"] = OperationStatus,
                ["OperationType"] = OperationType,
                ["OperationVersion"] = OperationVersion,
                ["OperationResult"] = OperationResult,
                ["OperationTarget"] = OperationTarget
            };

            if (AdditionalContext != null)
                dict["AdditionalContext"] = AdditionalContext;

            if (_metadata.Count > 0)
                dict["Metadata"] = _metadata;

            if (Tags != null && Tags.Count > 0)
                dict["Tags"] = Tags;

            if (OperationMetrics != null && OperationMetrics.Count > 0)
                dict["OperationMetrics"] = OperationMetrics;

            if (OperationDependencies != null && OperationDependencies.Count > 0)
                dict["OperationDependencies"] = OperationDependencies;

            if (OperationTags != null && OperationTags.Count > 0)
                dict["OperationTags"] = OperationTags;

            if (ServiceCalls != null && ServiceCalls.Count > 0)
                dict["ServiceCalls"] = ServiceCalls;

            if (DataFlows != null && DataFlows.Count > 0)
                dict["DataFlows"] = DataFlows;

            if (ComponentMetrics != null && ComponentMetrics.Count > 0)
                dict["ComponentMetrics"] = ComponentMetrics;

            if (ComponentDependencies != null && ComponentDependencies.Count > 0)
                dict["ComponentDependencies"] = ComponentDependencies;

            if (ContextData != null && ContextData.Count > 0)
                dict["ContextData"] = ContextData;

            return dict;
        }

        /// <summary>
        /// Adds an affected component to the error context.
        /// </summary>
        /// <param name="component">The affected component.</param>
        public void AddAffectedComponent(GraphNode component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            if (AffectedComponents == null)
                AffectedComponents = new List<GraphNode>();

            if (!AffectedComponents.Contains(component))
                AffectedComponents.Add(component);
        }

        /// <summary>
        /// Adds a previous error to the error context.
        /// </summary>
        /// <param name="error">The previous error.</param>
        public void AddPreviousError(ErrorContext error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            if (PreviousErrors == null)
                PreviousErrors = new List<ErrorContext>();

            if (!PreviousErrors.Contains(error))
                PreviousErrors.Add(error);
        }

        /// <summary>
        /// Adds a metric to the error context.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <param name="value">The metric value.</param>
        public void AddMetric(string name, double value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (Metrics == null)
                Metrics = new Dictionary<string, double>();

            Metrics[name] = value;
        }
    }
} 
