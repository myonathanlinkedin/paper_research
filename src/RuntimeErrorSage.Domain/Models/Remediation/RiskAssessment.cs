using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Remediation;

/// <summary>
/// Represents a risk assessment for a remediation operation.
/// </summary>
public class RiskAssessment
{
    /// <summary>
    /// Gets or sets the risk level.
    /// </summary>
    public RemediationRiskLevel RiskLevel { get; }

    /// <summary>
    /// Gets or sets a description of the risk.
    /// </summary>
    public string Description { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the potential issues that may arise.
    /// </summary>
    public IReadOnlyCollection<PotentialIssues> PotentialIssues { get; } = new Collection<string>();

    /// <summary>
    /// Gets or sets the mitigation strategies.
    /// </summary>
    public IReadOnlyCollection<MitigationStrategies> MitigationStrategies { get; } = new Collection<string>();

    /// <summary>
    /// Gets or sets the confidence level of the assessment (0-1).
    /// </summary>
    public double ConfidenceLevel { get; }

    /// <summary>
    /// Gets or sets whether the operation can be rolled back.
    /// </summary>
    public bool IsRollbackable { get; }

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
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the correlation ID for tracing.
    /// </summary>
    public string CorrelationId { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the probability of the risk occurring (0-1).
    /// </summary>
    public double Probability { get; }

    /// <summary>
    /// Gets or sets the impact severity if the risk occurs (0-1).
    /// </summary>
    public double Impact { get; }

    /// <summary>
    /// Gets or sets the list of mitigation steps.
    /// </summary>
    public IReadOnlyCollection<MitigationSteps> MitigationSteps { get; } = new();

    /// <summary>
    /// Gets or sets any additional metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the assessment notes.
    /// </summary>
    public string Notes { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the assessment.
    /// </summary>
    public AnalysisStatus Status { get; }

    /// <summary>
    /// Gets or sets the start time of the assessment.
    /// </summary>
    public DateTime StartTime { get; }

    /// <summary>
    /// Gets or sets the end time of the assessment.
    /// </summary>
    public DateTime EndTime { get; }

    /// <summary>
    /// Gets or sets the estimated duration of the remediation.
    /// </summary>
    public TimeSpan EstimatedDuration { get; }

    /// <summary>
    /// Gets or sets the list of affected components.
    /// </summary>
    public IReadOnlyCollection<AffectedComponents> AffectedComponents { get; } = new();

    /// <summary>
    /// Gets or sets the list of risk factors.
    /// </summary>
    public IReadOnlyCollection<RiskFactors> RiskFactors { get; } = new();

    /// <summary>
    /// Gets or sets the impact scope of the remediation.
    /// </summary>
    public RemediationActionImpactScope ImpactScope { get; }

    /// <summary>
    /// Gets or sets the list of warnings.
    /// </summary>
    public IReadOnlyCollection<Warnings> Warnings { get; } = new();

    public RiskAssessment()
    {
        Timestamp = DateTime.UtcNow;
        MitigationSteps = new Collection<string>();
        Metadata = new Dictionary<string, object>();
        Warnings = new Collection<string>();
        RiskLevel = RemediationRiskLevel.Medium;
        RiskFactors = new Collection<RiskFactor>();
    }
} 






