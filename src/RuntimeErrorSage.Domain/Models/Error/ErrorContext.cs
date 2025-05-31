using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Metrics;
using RuntimeErrorSage.Domain.Models;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Enums;
using System.Linq;
using RuntimeErrorSage.Domain.Interfaces;

using RuntimeErrorSage.Domain.Models.Flow;
using RuntimeErrorSage.Domain.Models.Error;


namespace RuntimeErrorSage.Domain.Models.Error
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
        public RuntimeError? Error { get; private set; }

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
        public string? ErrorSource { get; set; }

        /// <summary>
        /// Gets or sets the component graph data.
        /// </summary>
        public Dictionary<string, HashSet<string>> ComponentGraph { get; set; } = new();

        /// <summary>
        /// Gets or sets the dependency graph at the time of the error.
        /// </summary>
        public DependencyGraph? DependencyGraph { get; set; }

        /// <summary>
        /// Gets or sets the affected components in the dependency graph.
        /// </summary>
        public List<GraphNode>? AffectedComponents { get; set; }

        /// <summary>
        /// Gets or sets the metrics collected at the time of the error.
        /// </summary>
        public Dictionary<string, double>? Metrics { get; set; }

        /// <summary>
        /// Gets or sets the list of previous errors that may have contributed to this error.
        /// </summary>
        public List<ErrorContext>? PreviousErrors { get; set; }

        /// <summary>
        /// Gets or sets the analysis result of the error.
        /// </summary>
        public ErrorAnalysisResult? AnalysisResult { get; set; }

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
        /// Gets or sets the category of the error.
        /// </summary>
        public ErrorCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the severity level of the error.
        /// </summary>
        public SeverityLevel SeverityLevel { get; set; }

        /// <summary>
        /// Gets or sets the severity of the error.
        /// </summary>
        public SeverityLevel Severity { get; set; }

        /// <summary>
        /// Gets or sets the environment where the error occurred.
        /// </summary>
        public string Environment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source of the error.
        /// </summary>
        public string Source { get; set; } = string.Empty;

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
        public List<string> OperationTags { get; set; } = new();

        /// <summary>
        /// Gets or sets the service calls.
        /// </summary>
        public List<ServiceCall> ServiceCalls { get; set; } = new();

        /// <summary>
        /// Gets or sets the data flows.
        /// </summary>
        public List<DataFlow> DataFlows { get; set; } = new();

        /// <summary>
        /// Gets or sets the component metrics.
        /// </summary>
        public Dictionary<string, Dictionary<string, double>> ComponentMetrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the system state.
        /// </summary>
        public Dictionary<string, object> SystemState { get; set; } = new();

        /// <summary>
        /// Gets or sets the component dependencies.
        /// </summary>
        public List<ComponentDependency> ComponentDependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the context data.
        /// </summary>
        public Dictionary<string, object> ContextData { get; set; } = new();

        /// <summary>
        /// Gets or sets the inner error.
        /// </summary>
        public ErrorContext? InnerError { get; set; }

        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public string Context { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the error is actionable.
        /// </summary>
        public bool IsActionable { get; set; }

        /// <summary>
        /// Gets or sets whether the error is transient.
        /// </summary>
        public bool IsTransient { get; set; }

        /// <summary>
        /// Gets or sets whether the error is resolved.
        /// </summary>
        public bool IsResolved { get; set; }

        /// <summary>
        /// Gets or sets the remediation actions.
        /// </summary>
        public List<IRemediationAction> RemediationActions { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorContext"/> class.
        /// </summary>
        /// <param name="error">The runtime error.</param>
        /// <param name="context">The context information.</param>
        /// <param name="timestamp">The timestamp when the error occurred.</param>
        public ErrorContext(
            RuntimeError error,
            string context,
            DateTime timestamp)
        {
            ArgumentNullException.ThrowIfNull(error);
            ArgumentNullException.ThrowIfNull(context);

            Error = error;
            Context = context;
            Timestamp = timestamp;
            ErrorId = error.Id;
            Message = error.Message;
            ErrorType = error.ErrorType;
            StackTrace = error.StackTrace;
            ComponentId = error.ComponentId;
            
            foreach (var kvp in error.Metadata)
            {
                _metadata[kvp.Key] = kvp.Value;
            }
        }

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
        public object? GetMetadata(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return _metadata.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Gets typed metadata from the error context.
        /// </summary>
        /// <typeparam name="T">The type of the metadata.</typeparam>
        /// <param name="key">The metadata key.</param>
        /// <returns>The typed metadata value.</returns>
        public T? GetMetadata<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return _metadata.TryGetValue(key, out var value) ? (T)value : default;
        }

        /// <summary>
        /// Validates the error context.
        /// </summary>
        /// <returns>True if the error context is valid; otherwise, false.</returns>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(ErrorId))
                return false;

            if (string.IsNullOrEmpty(ComponentId))
                return false;

            if (string.IsNullOrEmpty(Message))
                return false;

            if (string.IsNullOrEmpty(ErrorType))
                return false;

            if (string.IsNullOrEmpty(Source))
                return false;

            if (string.IsNullOrEmpty(Environment))
                return false;

            if (string.IsNullOrEmpty(ServiceName))
                return false;

            if (string.IsNullOrEmpty(Context))
                return false;

            return true;
        }

        /// <summary>
        /// Converts the error context to a dictionary.
        /// </summary>
        /// <returns>The dictionary representation of the error context.</returns>
        public Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>
            {
                { "Id", Id },
                { "ErrorId", ErrorId },
                { "CorrelationId", CorrelationId },
                { "ComponentId", ComponentId },
                { "Message", Message },
                { "ErrorType", ErrorType },
                { "Severity", SeverityLevel },
                { "Environment", Environment },
                { "Source", Source },
                { "Category", Category },
                { "Tags", Tags },
                { "OperationName", OperationName },
                { "OperationId", OperationId },
                { "ParentOperationId", ParentOperationId },
                { "OperationStartTime", OperationStartTime },
                { "OperationDuration", OperationDuration },
                { "OperationStatus", OperationStatus },
                { "OperationType", OperationType },
                { "OperationVersion", OperationVersion },
                { "OperationResult", OperationResult },
                { "OperationTarget", OperationTarget },
                { "OperationMetrics", OperationMetrics },
                { "OperationDependencies", OperationDependencies },
                { "OperationTags", OperationTags },
                { "ServiceCalls", ServiceCalls },
                { "DataFlows", DataFlows },
                { "ComponentMetrics", ComponentMetrics },
                { "ComponentDependencies", ComponentDependencies },
                { "ContextData", ContextData },
                { "ServiceName", ServiceName },
                { "Context", Context },
                { "IsActionable", IsActionable },
                { "IsTransient", IsTransient },
                { "IsResolved", IsResolved }
            };

            if (Error != null)
            {
                dict["Error"] = Error;
                dict["StackTrace"] = StackTrace;
            }

            if (DependencyGraph != null)
            {
                dict["DependencyGraph"] = DependencyGraph;
            }

            if (AffectedComponents != null)
            {
                dict["AffectedComponents"] = AffectedComponents;
            }

            if (Metrics != null)
            {
                dict["Metrics"] = Metrics;
            }

            if (PreviousErrors != null)
            {
                dict["PreviousErrors"] = PreviousErrors;
            }

            if (AnalysisResult != null)
            {
                dict["AnalysisResult"] = AnalysisResult;
            }

            if (InnerError != null)
            {
                dict["InnerError"] = InnerError;
            }

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

            AffectedComponents ??= new List<GraphNode>();
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

            PreviousErrors ??= new List<ErrorContext>();
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

            Metrics ??= new Dictionary<string, double>();
            Metrics[name] = value;
        }

        /// <summary>
        /// Marks the error as resolved.
        /// </summary>
        /// <param name="resolutionDetails">The resolution details.</param>
        public void MarkAsResolved(string? resolutionDetails = null)
        {
            IsResolved = true;
            if (!string.IsNullOrEmpty(resolutionDetails))
            {
                AddMetadata("ResolutionDetails", resolutionDetails);
            }
        }

        /// <summary>
        /// Marks the error as actionable.
        /// </summary>
        /// <param name="actionDetails">The action details.</param>
        public void MarkAsActionable(string? actionDetails = null)
        {
            IsActionable = true;
            if (!string.IsNullOrEmpty(actionDetails))
            {
                AddMetadata("ActionDetails", actionDetails);
            }
        }

        /// <summary>
        /// Marks the error as transient.
        /// </summary>
        /// <param name="transientDetails">The transient details.</param>
        public void MarkAsTransient(string? transientDetails = null)
        {
            IsTransient = true;
            if (!string.IsNullOrEmpty(transientDetails))
            {
                AddMetadata("TransientDetails", transientDetails);
            }
        }

        /// <summary>
        /// Adds a relationship to the error context.
        /// </summary>
        /// <param name="relatedError">The related error.</param>
        /// <param name="relationshipType">The relationship type.</param>
        /// <param name="relationshipDetails">The relationship details.</param>
        public void AddRelationship(ErrorContext relatedError, string relationshipType, string? relationshipDetails = null)
        {
            if (relatedError == null)
                throw new ArgumentNullException(nameof(relatedError));

            if (string.IsNullOrEmpty(relationshipType))
                throw new ArgumentNullException(nameof(relationshipType));

            var relationship = new Dictionary<string, object>
            {
                { "RelatedErrorId", relatedError.ErrorId },
                { "RelationshipType", relationshipType },
                { "Timestamp", DateTime.UtcNow }
            };

            if (!string.IsNullOrEmpty(relationshipDetails))
            {
                relationship["Details"] = relationshipDetails;
            }

            AddMetadata($"Relationship_{relatedError.ErrorId}", relationship);
        }

        /// <summary>
        /// Gets the relationships from the error context.
        /// </summary>
        /// <returns>The list of relationships.</returns>
        public List<Dictionary<string, object>> GetRelationships()
        {
            return _metadata
                .Where(kvp => kvp.Key.StartsWith("Relationship_"))
                .Select(kvp => (Dictionary<string, object>)kvp.Value)
                .ToList();
        }

        /// <summary>
        /// Adds a dependency to the error context.
        /// </summary>
        /// <param name="dependentError">The dependent error.</param>
        /// <param name="dependencyType">The dependency type.</param>
        /// <param name="dependencyDetails">The dependency details.</param>
        public void AddDependency(ErrorContext dependentError, string dependencyType, string? dependencyDetails = null)
        {
            if (dependentError == null)
                throw new ArgumentNullException(nameof(dependentError));

            if (string.IsNullOrEmpty(dependencyType))
                throw new ArgumentNullException(nameof(dependencyType));

            var dependency = new Dictionary<string, object>
            {
                { "DependentErrorId", dependentError.ErrorId },
                { "DependencyType", dependencyType },
                { "Timestamp", DateTime.UtcNow }
            };

            if (!string.IsNullOrEmpty(dependencyDetails))
            {
                dependency["Details"] = dependencyDetails;
            }

            AddMetadata($"Dependency_{dependentError.ErrorId}", dependency);
        }

        /// <summary>
        /// Gets the dependencies from the error context.
        /// </summary>
        /// <returns>The list of dependencies.</returns>
        public List<Dictionary<string, object>> GetDependencies()
        {
            return _metadata
                .Where(kvp => kvp.Key.StartsWith("Dependency_"))
                .Select(kvp => (Dictionary<string, object>)kvp.Value)
                .ToList();
        }

        /// <summary>
        /// Adds an analysis result to the error context.
        /// </summary>
        /// <param name="analysisType">The analysis type.</param>
        /// <param name="analysisResult">The analysis result.</param>
        /// <param name="confidence">The confidence level.</param>
        public void AddAnalysisResult(string analysisType, object analysisResult, double confidence)
        {
            if (string.IsNullOrEmpty(analysisType))
                throw new ArgumentNullException(nameof(analysisType));

            if (analysisResult == null)
                throw new ArgumentNullException(nameof(analysisResult));

            var result = new Dictionary<string, object>
            {
                { "AnalysisType", analysisType },
                { "Result", analysisResult },
                { "Confidence", confidence },
                { "Timestamp", DateTime.UtcNow }
            };

            AddMetadata($"Analysis_{analysisType}", result);
        }

        /// <summary>
        /// Gets the analysis results from the error context.
        /// </summary>
        /// <returns>The list of analysis results.</returns>
        public List<Dictionary<string, object>> GetAnalysisResults()
        {
            return _metadata
                .Where(kvp => kvp.Key.StartsWith("Analysis_"))
                .Select(kvp => (Dictionary<string, object>)kvp.Value)
                .ToList();
        }
    }
} 

