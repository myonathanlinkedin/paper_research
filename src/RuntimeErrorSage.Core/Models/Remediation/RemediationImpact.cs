using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents the impact of a remediation operation.
/// </summary>
public class RemediationImpact
{
    /// <summary>
    /// Gets or sets the unique identifier of the impact assessment.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the correlation ID for tracing.
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the severity of the impact.
    /// </summary>
    public RemediationSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the risk level of the impact.
    /// </summary>
    public RiskLevel RiskLevel { get; set; } = RiskLevel.Medium;

    /// <summary>
    /// Gets or sets the impact scope.
    /// </summary>
    public RemediationActionImpactScope ImpactScope { get; set; }

    /// <summary>
    /// Gets or sets the affected components.
    /// </summary>
    public List<string> AffectedComponents { get; set; } = new();

    /// <summary>
    /// Gets or sets the estimated duration of the remediation.
    /// </summary>
    public TimeSpan EstimatedDuration { get; set; }

    /// <summary>
    /// Gets or sets the confidence level of the impact assessment (0-100).
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets the potential issues that may arise.
    /// </summary>
    public List<string> PotentialIssues { get; set; } = new();

    /// <summary>
    /// Gets or sets the mitigation strategies.
    /// </summary>
    public List<string> MitigationStrategies { get; set; } = new();

    /// <summary>
    /// Gets or sets any additional metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the timestamp of the impact assessment.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets whether the impact can be reversed.
    /// </summary>
    public bool IsReversible { get; set; }

    /// <summary>
    /// Gets or sets the additional notes about the impact.
    /// </summary>
    public string Notes { get; set; }

    /// <summary>
    /// Gets or sets the action ID.
    /// </summary>
    public string ActionId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the impact scope.
    /// </summary>
    public string Scope { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the impact description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the impact metrics.
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; } = new();
} 