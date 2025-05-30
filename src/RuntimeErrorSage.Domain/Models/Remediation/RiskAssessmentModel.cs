using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation;

/// <summary>
/// Represents a risk assessment for a remediation operation.
/// </summary>
public class RiskAssessmentModel
{
    /// <summary>
    /// Gets or sets the risk level.
    /// </summary>
    public RemediationRiskLevel RiskLevel { get; set; }

    /// <summary>
    /// Gets or sets a description of the risk.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the potential issues that may arise.
    /// </summary>
    public List<string> PotentialIssues { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the mitigation strategies.
    /// </summary>
    public List<string> MitigationStrategies { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the confidence level of the assessment (0-1).
    /// </summary>
    public double ConfidenceLevel { get; set; }

    /// <summary>
    /// Gets or sets whether the operation can be rolled back.
    /// </summary>
    public bool IsRollbackable { get; set; }

    /// <summary>
    /// Gets or sets the estimated time to rollback if needed.
    /// </summary>
    public TimeSpan? EstimatedRollbackTime { get; set; }

    /// <summary>
    /// Gets or sets additional risk factors.
    /// </summary>
    public Dictionary<string, object> Factors { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets the assessment timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the ID of the action being assessed.
    /// </summary>
    public string ActionId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the assessment was performed.
    /// </summary>
    public DateTime AssessedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the correlation ID for tracing.
    /// </summary>
    public string CorrelationId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the probability of the risk occurring (0-1).
    /// </summary>
    public double Probability { get; set; }

    /// <summary>
    /// Gets or sets the impact severity if the risk occurs (0-1).
    /// </summary>
    public double Impact { get; set; }

    /// <summary>
    /// Gets or sets the list of mitigation steps.
    /// </summary>
    public List<string> MitigationSteps { get; set; } = new();

    /// <summary>
    /// Gets or sets any additional metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

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

    public RiskAssessmentModel()
    {
        Timestamp = DateTime.UtcNow;
        AssessedAt = DateTime.UtcNow;
        MitigationSteps = new List<string>();
        Metadata = new Dictionary<string, object>();
        Warnings = new List<string>();
        RiskLevel = RemediationRiskLevel.Medium;
        RiskFactors = new List<RiskFactor>();
    }
} 
