using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Remediation.Base;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents the result of an error analysis.
/// </summary>
public class ErrorAnalysisResult
{
    /// <summary>
    /// Gets or sets the error identifier.
    /// </summary>
    public string ErrorId { get; set; }

    /// <summary>
    /// Gets or sets the correlation identifier.
    /// </summary>
    public string CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the analysis timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the analysis status.
    /// </summary>
    public AnalysisStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string ErrorType { get; set; }

    /// <summary>
    /// Gets or sets the error category.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Gets or sets the error severity.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the error description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the error context.
    /// </summary>
    public ErrorContext Context { get; set; }

    /// <summary>
    /// Gets or sets the error details.
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new();

    /// <summary>
    /// Gets or sets the fix complexity.
    /// </summary>
    public FixComplexity Complexity { get; set; }

    /// <summary>
    /// Gets or sets the priority level.
    /// </summary>
    public PreventionPriority Priority { get; set; }

    /// <summary>
    /// Gets or sets the estimated effectiveness.
    /// </summary>
    public double Effectiveness { get; set; }

    /// <summary>
    /// Gets or sets the implementation cost.
    /// </summary>
    public ImplementationCost Cost { get; set; }

    /// <summary>
    /// Gets or sets the root cause of the error.
    /// </summary>
    public string RootCause { get; set; }

    /// <summary>
    /// Gets or sets the accuracy of the analysis (0.0 to 1.0).
    /// </summary>
    public double Accuracy { get; set; }

    /// <summary>
    /// Gets or sets the latency of the analysis in milliseconds.
    /// </summary>
    public double Latency { get; set; }

    /// <summary>
    /// Gets or sets the memory usage of the analysis in bytes.
    /// </summary>
    public double MemoryUsage { get; set; }

    /// <summary>
    /// Gets or sets the CPU usage of the analysis as a percentage.
    /// </summary>
    public double CpuUsage { get; set; }

    /// <summary>
    /// Gets or sets the dependency graph.
    /// </summary>
    public DependencyGraph DependencyGraph { get; set; }

    /// <summary>
    /// Gets or sets the impact analysis results.
    /// </summary>
    public List<ImpactAnalysisResult> ImpactResults { get; set; } = new();

    /// <summary>
    /// Gets or sets the related errors.
    /// </summary>
    public List<RelatedError> RelatedErrors { get; set; } = new();

    /// <summary>
    /// Gets or sets the metrics.
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; } = new();

    /// <summary>
    /// Gets or sets the suggested remediation actions.
    /// </summary>
    public List<IRemediationAction> SuggestedActions { get; set; } = new();

    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the natural language explanation of the error.
    /// </summary>
    public string NaturalLanguageExplanation { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the root causes of the error.
    /// </summary>
    public List<string> RootCauses { get; set; } = new();

    /// <summary>
    /// Gets or sets the prevention strategies.
    /// </summary>
    public List<string> PreventionStrategies { get; set; } = new();

    /// <summary>
    /// Gets or sets the contextual data.
    /// </summary>
    public Dictionary<string, object> ContextualData { get; set; } = new();

    /// <summary>
    /// Gets or sets the remediation strategy.
    /// </summary>
    public RemediationStrategy RemediationStrategy { get; set; }

    /// <summary>
    /// Gets or sets whether auto-remediation is possible.
    /// </summary>
    public bool CanAutoRemediate { get; set; }

    /// <summary>
    /// Gets or sets the confidence score (0-1).
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets whether there was an error during analysis.
    /// </summary>
    public string AnalysisError { get; set; }

    /// <summary>
    /// Gets or sets whether the analysis is complete.
    /// </summary>
    public bool IsComplete { get; set; }

    /// <summary>
    /// Gets or sets additional metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 
