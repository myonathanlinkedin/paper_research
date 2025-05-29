using System.Collections.ObjectModel;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Graph;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;

namespace RuntimeErrorSage.Application.Models.Error;

/// <summary>
/// Represents the result of an error analysis.
/// </summary>
public class ErrorAnalysisResult
{
    /// <summary>
    /// Gets or sets the error identifier.
    /// </summary>
    public string ErrorId { get; }

    /// <summary>
    /// Gets or sets the correlation identifier.
    /// </summary>
    public string CorrelationId { get; }

    /// <summary>
    /// Gets or sets the analysis timestamp.
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// Gets or sets the analysis status.
    /// </summary>
    public AnalysisStatus Status { get; }

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string ErrorType { get; }

    /// <summary>
    /// Gets or sets the error category.
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Gets or sets the error severity.
    /// </summary>
    public ErrorSeverity Severity { get; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets or sets the error description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets or sets the error context.
    /// </summary>
    public ErrorContext Context { get; }

    /// <summary>
    /// Gets or sets the error details.
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new();

    /// <summary>
    /// Gets or sets the fix complexity.
    /// </summary>
    public FixComplexity Complexity { get; }

    /// <summary>
    /// Gets or sets the priority level.
    /// </summary>
    public PreventionPriority Priority { get; }

    /// <summary>
    /// Gets or sets the estimated effectiveness.
    /// </summary>
    public double Effectiveness { get; }

    /// <summary>
    /// Gets or sets the implementation cost.
    /// </summary>
    public ImplementationCost Cost { get; }

    /// <summary>
    /// Gets or sets the root cause of the error.
    /// </summary>
    public string RootCause { get; }

    /// <summary>
    /// Gets or sets the accuracy of the analysis (0.0 to 1.0).
    /// </summary>
    public double Accuracy { get; }

    /// <summary>
    /// Gets or sets the latency of the analysis in milliseconds.
    /// </summary>
    public double Latency { get; }

    /// <summary>
    /// Gets or sets the memory usage of the analysis in bytes.
    /// </summary>
    public double MemoryUsage { get; }

    /// <summary>
    /// Gets or sets the CPU usage of the analysis as a percentage.
    /// </summary>
    public double CpuUsage { get; }

    /// <summary>
    /// Gets or sets the dependency graph.
    /// </summary>
    public DependencyGraph DependencyGraph { get; }

    /// <summary>
    /// Gets or sets the impact analysis results.
    /// </summary>
    public IReadOnlyCollection<ImpactResults> ImpactResults { get; } = new();

    /// <summary>
    /// Gets or sets the related errors.
    /// </summary>
    public IReadOnlyCollection<RelatedErrors> RelatedErrors { get; } = new();

    /// <summary>
    /// Gets or sets the metrics.
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; } = new();

    /// <summary>
    /// Gets or sets the suggested remediation actions.
    /// </summary>
    public IReadOnlyCollection<SuggestedActions> SuggestedActions { get; } = new();

    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the natural language explanation of the error.
    /// </summary>
    public string NaturalLanguageExplanation { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the root causes of the error.
    /// </summary>
    public IReadOnlyCollection<RootCauses> RootCauses { get; } = new();

    /// <summary>
    /// Gets or sets the prevention strategies.
    /// </summary>
    public IReadOnlyCollection<PreventionStrategies> PreventionStrategies { get; } = new();

    /// <summary>
    /// Gets or sets the contextual data.
    /// </summary>
    public Dictionary<string, object> ContextualData { get; set; } = new();

    /// <summary>
    /// Gets or sets whether auto-remediation is possible.
    /// </summary>
    public bool CanAutoRemediate { get; }

    /// <summary>
    /// Gets or sets the confidence score (0-1).
    /// </summary>
    public double Confidence { get; }

    /// <summary>
    /// Gets or sets whether there was an error during analysis.
    /// </summary>
    public string AnalysisError { get; }

    /// <summary>
    /// Gets or sets whether the analysis is complete.
    /// </summary>
    public bool IsComplete { get; }

    /// <summary>
    /// Gets or sets additional metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 






