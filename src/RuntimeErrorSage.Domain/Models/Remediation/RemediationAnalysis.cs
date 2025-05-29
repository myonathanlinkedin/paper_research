using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.Graph;
using RuntimeErrorSage.Model.Models.LLM;
using RuntimeErrorSage.Model.Models.Remediation.Interfaces;
using RuntimeErrorSage.Model.Models.Context;

namespace RuntimeErrorSage.Model.Models.Remediation;

/// <summary>
/// Represents an analysis of a remediation operation.
/// </summary>
public class RemediationAnalysis
{
    /// <summary>
    /// Gets or sets the analysis identifier.
    /// </summary>
    public string AnalysisId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the error context.
    /// </summary>
    public ErrorContext ErrorContext { get; set; }

    /// <summary>
    /// Gets or sets the analysis timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the analysis confidence score.
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets the analysis severity.
    /// </summary>
    public RemediationActionSeverity Severity { get; set; } = RemediationActionSeverity.Medium;

    /// <summary>
    /// Gets or sets the analysis priority.
    /// </summary>
    public RemediationPriority Priority { get; set; }

    /// <summary>
    /// Gets or sets the analysis impact.
    /// </summary>
    public RemediationImpact Impact { get; set; }

    /// <summary>
    /// Gets or sets the applicable strategies for this analysis.
    /// </summary>
    public List<StrategyRecommendation> ApplicableStrategies { get; set; } = new();

    /// <summary>
    /// Gets or sets the graph analysis result.
    /// </summary>
    public GraphAnalysis GraphAnalysis { get; set; }

    /// <summary>
    /// Gets or sets the LLM analysis result.
    /// </summary>
    public LLMAnalysis LLMAnalysis { get; set; }

    /// <summary>
    /// Gets or sets whether the analysis is valid.
    /// </summary>
    public bool IsValid { get; set; }

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
    public List<RemediationAction> SuggestedActions { get; set; } = new();

    /// <summary>
    /// Gets or sets the analysis status.
    /// </summary>
    public RemediationStatusInfo Status { get; set; }

    /// <summary>
    /// Gets or sets the error message if analysis failed.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the runtime context.
    /// </summary>
    public RuntimeContext RuntimeContext { get; set; }

    /// <summary>
    /// Gets or sets the remediation steps.
    /// </summary>
    public List<string> RemediationSteps { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the remediation result.
    /// </summary>
    public RemediationResult Result { get; set; }
} 
