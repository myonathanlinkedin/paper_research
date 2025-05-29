using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Graph;
using RuntimeErrorSage.Application.Models.LLM;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Models.Context;

namespace RuntimeErrorSage.Application.Models.Remediation;

/// <summary>
/// Represents an analysis of a remediation operation.
/// </summary>
public class RemediationAnalysis
{
    /// <summary>
    /// Gets or sets the analysis identifier.
    /// </summary>
    public string AnalysisId { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the error context.
    /// </summary>
    public ErrorContext ErrorContext { get; }

    /// <summary>
    /// Gets or sets the analysis timestamp.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the analysis confidence score.
    /// </summary>
    public double Confidence { get; }

    /// <summary>
    /// Gets or sets the analysis severity.
    /// </summary>
    public RemediationActionSeverity Severity { get; } = RemediationActionSeverity.Medium;

    /// <summary>
    /// Gets or sets the analysis priority.
    /// </summary>
    public RemediationPriority Priority { get; }

    /// <summary>
    /// Gets or sets the analysis impact.
    /// </summary>
    public RemediationImpact Impact { get; }

    /// <summary>
    /// Gets or sets the applicable strategies for this analysis.
    /// </summary>
    public IReadOnlyCollection<ApplicableStrategies> ApplicableStrategies { get; } = new();

    /// <summary>
    /// Gets or sets the graph analysis result.
    /// </summary>
    public GraphAnalysis GraphAnalysis { get; }

    /// <summary>
    /// Gets or sets the LLM analysis result.
    /// </summary>
    public LLMAnalysis LLMAnalysis { get; }

    /// <summary>
    /// Gets or sets whether the analysis is valid.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Gets or sets the analysis metrics.
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; } = new();

    /// <summary>
    /// Gets or sets the analysis metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the suggested actions.
    /// </summary>
    public IReadOnlyCollection<SuggestedActions> SuggestedActions { get; } = new();

    /// <summary>
    /// Gets or sets the analysis status.
    /// </summary>
    public RemediationStatusInfo Status { get; }

    /// <summary>
    /// Gets or sets the error message if analysis failed.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Gets or sets the runtime context.
    /// </summary>
    public RuntimeContext RuntimeContext { get; }

    /// <summary>
    /// Gets or sets the remediation steps.
    /// </summary>
    public IReadOnlyCollection<RemediationSteps> RemediationSteps { get; } = new Collection<string>();

    /// <summary>
    /// Gets or sets the remediation result.
    /// </summary>
    public RemediationResult Result { get; }
} 




