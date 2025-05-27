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
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            Error = error;
            Environment = environment;
            Timestamp = timestamp ?? DateTime.UtcNow;
            AffectedComponents = new List<GraphNode>();
            Metrics = new Dictionary<string, double>();
            PreviousErrors = new List<ErrorContext>();
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Error Error { get; }

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
                { nameof(OperationTags), OperationTags },
                { nameof(DependencyGraph), DependencyGraph },
                { nameof(AffectedComponents), AffectedComponents },
                { nameof(Metrics), Metrics },
                { nameof(PreviousErrors), PreviousErrors },
                { nameof(AnalysisResult), AnalysisResult },
                { nameof(ServiceCalls), ServiceCalls },
                { nameof(DataFlows), DataFlows },
                { nameof(ComponentMetrics), ComponentMetrics },
                { nameof(ComponentDependencies), ComponentDependencies },
                { nameof(ContextData), ContextData },
                { nameof(InnerError), InnerError }
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

        /// <summary>
        /// Adds a component to the list of affected components.
        /// </summary>
        /// <param name="component">The component to add.</param>
        public void AddAffectedComponent(GraphNode component)
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            if (!AffectedComponents.Contains(component))
            {
                AffectedComponents.Add(component);
            }
        }

        /// <summary>
        /// Adds a previous error to the list of previous errors.
        /// </summary>
        /// <param name="error">The error to add.</param>
        public void AddPreviousError(ErrorContext error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            if (!PreviousErrors.Contains(error))
            {
                PreviousErrors.Add(error);
            }
        }

        /// <summary>
        /// Adds a metric to the metrics dictionary.
        /// </summary>
        /// <param name="name">The name of the metric.</param>
        /// <param name="value">The value of the metric.</param>
        public void AddMetric(string name, double value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Metric name cannot be null or empty.", nameof(name));
            }

            Metrics[name] = value;
        }
    }

    // Add definitions for ServiceCall, DataFlow, ComponentDependency if not present
    public class ServiceCall
    {
        public string Source { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool Success { get; set; }
    }

    public class DataFlow
    {
        public string Source { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public double Volume { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class ComponentDependency
    {
        public string Source { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string DependencyType { get; set; } = string.Empty;
        public double Strength { get; set; }
    }
} 
