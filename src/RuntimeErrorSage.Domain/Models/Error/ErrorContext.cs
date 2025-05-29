using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Metrics;
using RuntimeErrorSage.Application.Models;
using RuntimeErrorSage.Application.Models.Graph;
using RuntimeErrorSage.Domain.Enums;
using System.Linq;

namespace RuntimeErrorSage.Application.Models.Error
{
    /// <summary>
    /// Represents the context of an error, including exception details and additional metadata.
    /// </summary>
    public class ErrorContext
    {
        /// <summary>
        /// Gets or sets the unique identifier for this error context.
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error identifier.
        /// </summary>
        public string ErrorId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the correlation identifier for tracking related errors.
        /// </summary>
        public string CorrelationId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the component identifier where the error occurred.
        /// </summary>
        public string ComponentId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the exception that caused the error.
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// Gets or sets the stack trace of the error.
        /// </summary>
        public string StackTrace { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the error occurred.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

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
        public Collection<GraphNode>? AffectedComponents { get; set; }

        /// <summary>
        /// Gets or sets the metrics collected at the time of the error.
        /// </summary>
        public Dictionary<string, double>? Metrics { get; set; }

        /// <summary>
        /// Gets or sets the list of previous errors that may have contributed to this error.
        /// </summary>
        public Collection<ErrorContext>? PreviousErrors { get; set; }

        /// <summary>
        /// Gets or sets the analysis result of the error.
        /// </summary>
        public ErrorAnalysisResult? AnalysisResult { get; set; }

        /// <summary>
        /// Gets or sets the context identifier.
        /// </summary>
        public string ContextId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string ComponentName { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string ErrorType { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the severity level of the error.
        /// </summary>
        public SeverityLevel Severity { get; }

        /// <summary>
        /// Gets or sets the environment where the error occurred.
        /// </summary>
        public string Environment { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the source of the error.
        /// </summary>
        public string Source { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the error category.
        /// </summary>
        public ErrorCategory Category { get; }

        /// <summary>
        /// Gets or sets the error tags.
        /// </summary>
        public IReadOnlyCollection<Tags> Tags { get; } = new();

        /// <summary>
        /// Gets or sets the operation name.
        /// </summary>
        public string OperationName { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation ID.
        /// </summary>
        public string OperationId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the parent operation ID.
        /// </summary>
        public string ParentOperationId { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation start time.
        /// </summary>
        public DateTime OperationStartTime { get; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the operation duration.
        /// </summary>
        public TimeSpan OperationDuration { get; }

        /// <summary>
        /// Gets or sets the operation status.
        /// </summary>
        public string OperationStatus { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation type.
        /// </summary>
        public string OperationType { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation version.
        /// </summary>
        public string OperationVersion { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation result.
        /// </summary>
        public string OperationResult { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation target.
        /// </summary>
        public string OperationTarget { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation metrics.
        /// </summary>
        public Dictionary<string, double> OperationMetrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the operation dependencies.
        /// </summary>
        public IReadOnlyCollection<OperationDependencies> OperationDependencies { get; } = new();

        /// <summary>
        /// Gets or sets the operation tags.
        /// </summary>
        public Dictionary<string, string> OperationTags { get; set; } = new();

        /// <summary>
        /// Gets or sets the service calls related to this error context.
        /// </summary>
        public IReadOnlyCollection<ServiceCalls> ServiceCalls { get; } = new();

        /// <summary>
        /// Gets or sets the data flows related to this error context.
        /// </summary>
        public IReadOnlyCollection<DataFlows> DataFlows { get; } = new();

        /// <summary>
        /// Gets or sets the component metrics for this error context.
        /// </summary>
        public Dictionary<string, Dictionary<string, double>> ComponentMetrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the component dependencies for this error context.
        /// </summary>
        public IReadOnlyCollection<ComponentDependencies> ComponentDependencies { get; } = new();

        /// <summary>
        /// Gets or sets additional error context data.
        /// </summary>
        public Dictionary<string, object> ContextData { get; set; } = new();

        /// <summary>
        /// Gets or sets the inner error context if any.
        /// </summary>
        public ErrorContext? InnerError { get; set; }

        /// <summary>
        /// Gets or sets the name of the service where the error occurred.
        /// </summary>
        public string ServiceName { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the error context information.
        /// </summary>
        public string Context { get; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the error is actionable.
        /// </summary>
        public bool IsActionable { get; }

        /// <summary>
        /// Gets or sets whether the error is transient.
        /// </summary>
        public bool IsTransient { get; }

        /// <summary>
        /// Gets or sets whether the error has been resolved.
        /// </summary>
        public bool IsResolved { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorContext"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="context">The context.</param>
        /// <param name="timestamp">The timestamp.</param>
        public ErrorContext(
            RuntimeError error,
            string context,
            DateTime timestamp)
        {
            Error = error;
            Context = context;
            Timestamp = timestamp;
            ErrorId = error.Id;
            Message = error.Message;
            ErrorType = error.GetType().Name;
            StackTrace = error.StackTrace ?? string.Empty;
            Exception = error;
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public RuntimeError? Error { get; }

        /// <summary>
        /// Adds metadata to the error context.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <param name="value">The metadata value.</param>
        public string key, object value { ArgumentNullException.ThrowIfNull(string key, object value); }
        {
            ArgumentNullException.ThrowIfNull(key);
            _metadata[key] = value;
        }

        /// <summary>
        /// Gets metadata from the error context.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <returns>The metadata value.</returns>
        public string key { ArgumentNullException.ThrowIfNull(string key); }
        {
            ArgumentNullException.ThrowIfNull(key);
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
            ArgumentNullException.ThrowIfNull(key);
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

            if (string.IsNullOrEmpty(ErrorType))
                return false;

            if (string.IsNullOrEmpty(Source))
                return false;

            if (string.IsNullOrEmpty(Environment))
                return false;

            if (Timestamp == default)
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
                ["ComponentName"] = ComponentName,
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
                ["OperationTarget"] = OperationTarget,
                ["Context"] = Context,
                ["IsActionable"] = IsActionable,
                ["IsTransient"] = IsTransient,
                ["IsResolved"] = IsResolved
            };

            if (AdditionalContext?.Count > 0)
                dict["AdditionalContext"] = AdditionalContext;

            if (_metadata?.Count > 0)
                dict["Metadata"] = _metadata;

            if (Tags?.Count > 0)
                dict["Tags"] = Tags;

            if (OperationMetrics?.Count > 0)
                dict["OperationMetrics"] = OperationMetrics;

            if (OperationDependencies?.Count > 0)
                dict["OperationDependencies"] = OperationDependencies;

            if (OperationTags?.Count > 0)
                dict["OperationTags"] = OperationTags;

            if (ServiceCalls?.Count > 0)
                dict["ServiceCalls"] = ServiceCalls;

            if (DataFlows?.Count > 0)
                dict["DataFlows"] = DataFlows;

            if (ComponentMetrics?.Count > 0)
                dict["ComponentMetrics"] = ComponentMetrics;

            if (ComponentDependencies?.Count > 0)
                dict["ComponentDependencies"] = ComponentDependencies;

            if (ContextData?.Count > 0)
                dict["ContextData"] = ContextData;

            if (AffectedComponents?.Count > 0)
                dict["AffectedComponents"] = AffectedComponents;

            if (PreviousErrors?.Count > 0)
                dict["PreviousErrors"] = PreviousErrors;

            if (AnalysisResult != null)
                dict["AnalysisResult"] = AnalysisResult;

            return dict;
        }

        /// <summary>
        /// Adds an affected component to the error context.
        /// </summary>
        /// <param name="component">The affected component.</param>
        public GraphNode component { ArgumentNullException.ThrowIfNull(GraphNode component); }
        {
            ArgumentNullException.ThrowIfNull(component);

            if (AffectedComponents == null)
                AffectedComponents = new Collection<GraphNode>();

            if (!AffectedComponents.Contains(component))
                AffectedComponents.Add(component);
        }

        /// <summary>
        /// Adds a previous error to the error context.
        /// </summary>
        /// <param name="error">The previous error.</param>
        public ErrorContext error { ArgumentNullException.ThrowIfNull(ErrorContext error); }
        {
            ArgumentNullException.ThrowIfNull(error);

            if (PreviousErrors == null)
                PreviousErrors = new Collection<ErrorContext>();

            if (!PreviousErrors.Contains(error))
                PreviousErrors.Add(error);
        }

        /// <summary>
        /// Adds a metric to the error context.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <param name="value">The metric value.</param>
        public string name, double value { ArgumentNullException.ThrowIfNull(string name, double value); }
        {
            ArgumentNullException.ThrowIfNull(name);

            if (Metrics == null)
                Metrics = new Dictionary<string, double>();

            Metrics[name] = value;
        }

        /// <summary>
        /// Marks the error as resolved and updates related metadata.
        /// </summary>
        /// <param name="resolutionDetails">Optional details about the resolution.</param>
        public string resolutionDetails = null { ArgumentNullException.ThrowIfNull(string resolutionDetails = null); }
        {
            IsResolved = true;
            IsActionable = false;
            
            if (!string.IsNullOrEmpty(resolutionDetails))
            {
                AddMetadata("ResolutionDetails", resolutionDetails);
                AddMetadata("ResolutionTimestamp", DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Marks the error as actionable and updates related metadata.
        /// </summary>
        /// <param name="actionDetails">Optional details about the required action.</param>
        public string actionDetails = null { ArgumentNullException.ThrowIfNull(string actionDetails = null); }
        {
            IsActionable = true;
            
            if (!string.IsNullOrEmpty(actionDetails))
            {
                AddMetadata("ActionDetails", actionDetails);
                AddMetadata("ActionRequiredTimestamp", DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Marks the error as transient and updates related metadata.
        /// </summary>
        /// <param name="transientDetails">Optional details about the transient nature.</param>
        public string transientDetails = null { ArgumentNullException.ThrowIfNull(string transientDetails = null); }
        {
            IsTransient = true;
            
            if (!string.IsNullOrEmpty(transientDetails))
            {
                AddMetadata("TransientDetails", transientDetails);
                AddMetadata("TransientMarkedTimestamp", DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Adds a correlation between this error and another error.
        /// </summary>
        /// <param name="relatedError">The related error context.</param>
        /// <param name="correlationType">The type of correlation.</param>
        /// <param name="correlationDetails">Optional details about the correlation.</param>
        public ErrorContext relatedError, string correlationType, string correlationDetails = null { ArgumentNullException.ThrowIfNull(ErrorContext relatedError, string correlationType, string correlationDetails = null); }
        {
            ArgumentNullException.ThrowIfNull(relatedError);
            ArgumentNullException.ThrowIfNull(correlationType);

            var correlation = new Dictionary<string, object>
            {
                ["RelatedErrorId"] = relatedError.ErrorId,
                ["CorrelationType"] = correlationType,
                ["Timestamp"] = DateTime.UtcNow
            };

            if (!string.IsNullOrEmpty(correlationDetails))
            {
                correlation["Details"] = correlationDetails;
            }

            var correlations = GetMetadata<Collection<Dictionary<string, object>>>("Correlations") ?? new Collection<Dictionary<string, object>>();
            correlations.Add(correlation);
            AddMetadata("Correlations", correlations);
        }

        /// <summary>
        /// Gets all correlations for this error.
        /// </summary>
        /// <returns>A list of correlation dictionaries.</returns>
        public Collection<Dictionary<string, object>> GetCorrelations()
        {
            return GetMetadata<Collection<Dictionary<string, object>>>("Correlations") ?? new Collection<Dictionary<string, object>>();
        }

        /// <summary>
        /// Adds a relationship between this error and another error.
        /// </summary>
        /// <param name="relatedError">The related error context.</param>
        /// <param name="relationshipType">The type of relationship.</param>
        /// <param name="relationshipDetails">Optional details about the relationship.</param>
        public ErrorContext relatedError, string relationshipType, string relationshipDetails = null { ArgumentNullException.ThrowIfNull(ErrorContext relatedError, string relationshipType, string relationshipDetails = null); }
        {
            ArgumentNullException.ThrowIfNull(relatedError);
            ArgumentNullException.ThrowIfNull(relationshipType);

            var relationship = new Dictionary<string, object>
            {
                ["RelatedErrorId"] = relatedError.ErrorId,
                ["RelationshipType"] = relationshipType,
                ["Timestamp"] = DateTime.UtcNow
            };

            if (!string.IsNullOrEmpty(relationshipDetails))
            {
                relationship["Details"] = relationshipDetails;
            }

            var relationships = GetMetadata<Collection<Dictionary<string, object>>>("Relationships") ?? new Collection<Dictionary<string, object>>();
            relationships.Add(relationship);
            AddMetadata("Relationships", relationships);
        }

        /// <summary>
        /// Gets all relationships for this error.
        /// </summary>
        /// <returns>A list of relationship dictionaries.</returns>
        public Collection<Dictionary<string, object>> GetRelationships()
        {
            return GetMetadata<Collection<Dictionary<string, object>>>("Relationships") ?? new Collection<Dictionary<string, object>>();
        }

        /// <summary>
        /// Adds a dependency between this error and another error.
        /// </summary>
        /// <param name="dependentError">The dependent error context.</param>
        /// <param name="dependencyType">The type of dependency.</param>
        /// <param name="dependencyDetails">Optional details about the dependency.</param>
        public ErrorContext dependentError, string dependencyType, string dependencyDetails = null { ArgumentNullException.ThrowIfNull(ErrorContext dependentError, string dependencyType, string dependencyDetails = null); }
        {
            ArgumentNullException.ThrowIfNull(dependentError);
            ArgumentNullException.ThrowIfNull(dependencyType);

            var dependency = new Dictionary<string, object>
            {
                ["DependentErrorId"] = dependentError.ErrorId,
                ["DependencyType"] = dependencyType,
                ["Timestamp"] = DateTime.UtcNow
            };

            if (!string.IsNullOrEmpty(dependencyDetails))
            {
                dependency["Details"] = dependencyDetails;
            }

            var dependencies = GetMetadata<Collection<Dictionary<string, object>>>("Dependencies") ?? new Collection<Dictionary<string, object>>();
            dependencies.Add(dependency);
            AddMetadata("Dependencies", dependencies);
        }

        /// <summary>
        /// Gets all dependencies for this error.
        /// </summary>
        /// <returns>A list of dependency dictionaries.</returns>
        public Collection<Dictionary<string, object>> GetDependencies()
        {
            return GetMetadata<Collection<Dictionary<string, object>>>("Dependencies") ?? new Collection<Dictionary<string, object>>();
        }

        /// <summary>
        /// Adds an analysis result to the error context.
        /// </summary>
        /// <param name="analysisType">The type of analysis.</param>
        /// <param name="analysisResult">The analysis result.</param>
        /// <param name="confidence">The confidence level of the analysis (0-1).</param>
        public string analysisType, object analysisResult, double confidence { ArgumentNullException.ThrowIfNull(string analysisType, object analysisResult, double confidence); }
        {
            ArgumentNullException.ThrowIfNull(analysisType);
            ArgumentNullException.ThrowIfNull(analysisResult);

            if (confidence < 0 || confidence > 1)
                throw new ArgumentOutOfRangeException(nameof(confidence), "Confidence must be between 0 and 1");

            var analysis = new Dictionary<string, object>
            {
                ["AnalysisType"] = analysisType,
                ["Result"] = analysisResult,
                ["Confidence"] = confidence,
                ["Timestamp"] = DateTime.UtcNow
            };

            var analyses = GetMetadata<Collection<Dictionary<string, object>>>("Analyses") ?? new Collection<Dictionary<string, object>>();
            analyses.Add(analysis);
            AddMetadata("Analyses", analyses);
        }

        /// <summary>
        /// Gets all analysis results for this error.
        /// </summary>
        /// <returns>A list of analysis result dictionaries.</returns>
        public Collection<Dictionary<string, object>> GetAnalysisResults()
        {
            return GetMetadata<Collection<Dictionary<string, object>>>("Analyses") ?? new Collection<Dictionary<string, object>>();
        }

        /// <summary>
        /// Adds a performance metric to the error context.
        /// </summary>
        /// <param name="metricName">The name of the metric.</param>
        /// <param name="value">The metric value.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <param name="timestamp">Optional timestamp for the metric.</param>
        public string metricName, double value, string unit, DateTime? timestamp = null { ArgumentNullException.ThrowIfNull(string metricName, double value, string unit, DateTime? timestamp = null); }
        {
            if (string.IsNullOrEmpty(metricName))
                ArgumentNullException.ThrowIfNull(nameof(metricName));

            if (string.IsNullOrEmpty(unit))
                ArgumentNullException.ThrowIfNull(nameof(unit));

            var metric = new Dictionary<string, object>
            {
                ["MetricName"] = metricName,
                ["Value"] = value,
                ["Unit"] = unit,
                ["Timestamp"] = timestamp ?? DateTime.UtcNow
            };

            var metrics = GetMetadata<Collection<Dictionary<string, object>>>("PerformanceMetrics") ?? new Collection<Dictionary<string, object>>();
            metrics.Add(metric);
            AddMetadata("PerformanceMetrics", metrics);
        }

        /// <summary>
        /// Gets all performance metrics for this error.
        /// </summary>
        /// <returns>A list of performance metric dictionaries.</returns>
        public Collection<Dictionary<string, object>> GetPerformanceMetrics()
        {
            return GetMetadata<Collection<Dictionary<string, object>>>("PerformanceMetrics") ?? new Collection<Dictionary<string, object>>();
        }

        /// <summary>
        /// Adds a system state snapshot to the error context.
        /// </summary>
        /// <param name="stateData">The system state data.</param>
        /// <param name="timestamp">Optional timestamp for the snapshot.</param>
        public Dictionary<string, object> stateData, DateTime? timestamp = null { ArgumentNullException.ThrowIfNull(Dictionary<string, object> stateData, DateTime? timestamp = null); }
        {
            if (stateData == null)
                ArgumentNullException.ThrowIfNull(nameof(stateData));

            var snapshot = new Dictionary<string, object>
            {
                ["StateData"] = stateData,
                ["Timestamp"] = timestamp ?? DateTime.UtcNow
            };

            var snapshots = GetMetadata<Collection<Dictionary<string, object>>>("SystemStateSnapshots") ?? new Collection<Dictionary<string, object>>();
            snapshots.Add(snapshot);
            AddMetadata("SystemStateSnapshots", snapshots);
        }

        /// <summary>
        /// Gets all system state snapshots for this error.
        /// </summary>
        /// <returns>A list of system state snapshot dictionaries.</returns>
        public Collection<Dictionary<string, object>> GetSystemStateSnapshots()
        {
            return GetMetadata<Collection<Dictionary<string, object>>>("SystemStateSnapshots") ?? new Collection<Dictionary<string, object>>();
        }

        /// <summary>
        /// Calculates the error impact score based on various factors.
        /// </summary>
        /// <returns>A score between 0 and 1 indicating the error's impact.</returns>
        public double CalculateImpactScore()
        {
            double score = 0;
            
            // Severity impact (0-1)
            score += (int)Severity / 6.0; // SeverityLevel has 6 levels (0-5)
            
            // Affected components impact (0-1)
            if (AffectedComponents?.Any() == true)
            {
                score += Math.Min(AffectedComponents.Count / 10.0, 1.0);
            }
            
            // Dependencies impact (0-1)
            if (DependencyGraph?.Nodes?.Any() == true)
            {
                score += Math.Min(DependencyGraph.Nodes.Count / 10.0, 1.0);
            }
            
            // Performance impact (0-1)
            if (Metrics?.Any() == true)
            {
                var performanceMetrics = Metrics.Where(m => m.Key.StartsWith("Performance", StringComparison.OrdinalIgnoreCase));
                if (performanceMetrics.Any())
                {
                    score += Math.Min(performanceMetrics.Count() / 5.0, 1.0);
                }
            }
            
            // Normalize to 0-1 range
            return Math.Min(score / 4.0, 1.0);
        }

        public Dictionary<string, object> stateData { ArgumentNullException.ThrowIfNull(Dictionary<string, object> stateData); }
        {
            ArgumentNullException.ThrowIfNull(stateData);
            AddMetadata("State", stateData);
        }
    }
} 





