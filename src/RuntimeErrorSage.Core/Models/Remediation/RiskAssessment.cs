using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents a risk assessment for a remediation action.
/// </summary>
public class RiskAssessment
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the correlation ID for tracing.
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the risk level.
    /// </summary>
    public RiskLevel RiskLevel { get; set; }

    /// <summary>
    /// Gets or sets the probability of the risk occurring (0-1).
    /// </summary>
    public double Probability { get; set; }

    /// <summary>
    /// Gets or sets the impact severity if the risk occurs (0-1).
    /// </summary>
    public double Impact { get; set; }

    /// <summary>
    /// Gets or sets the potential issues that may arise.
    /// </summary>
    public List<string> PotentialIssues { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of mitigation steps.
    /// </summary>
    public List<string> MitigationSteps { get; set; } = new();

    /// <summary>
    /// Gets or sets the confidence level of the assessment (0-100).
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets any additional metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the timestamp of the assessment.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the assessment notes.
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the assessment.
    /// </summary>
    public AnalysisStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the start time of the assessment.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the assessment.
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Gets or sets the estimated duration of the remediation.
    /// </summary>
    public TimeSpan EstimatedDuration { get; set; }

    /// <summary>
    /// Gets or sets the list of affected components.
    /// </summary>
    public List<string> AffectedComponents { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of risk factors.
    /// </summary>
    public List<RiskFactor> RiskFactors { get; set; } = new();

    /// <summary>
    /// Gets or sets the impact scope of the remediation.
    /// </summary>
    public RemediationActionImpactScope ImpactScope { get; set; }

    /// <summary>
    /// Gets or sets the list of warnings.
    /// </summary>
    public List<string> Warnings { get; set; } = new();

    public RiskAssessment()
    {
        Timestamp = DateTime.UtcNow;
        MitigationSteps = new List<string>();
        Metadata = new Dictionary<string, object>();
        Warnings = new List<string>();
        RiskLevel = RiskLevel.Medium;
        RiskFactors = new List<RiskFactor>();
    }
} 