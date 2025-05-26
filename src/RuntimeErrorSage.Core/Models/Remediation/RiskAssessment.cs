using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents a risk assessment for a remediation operation.
/// </summary>
public class RiskAssessment
{
    /// <summary>
    /// Gets or sets the unique identifier for this assessment.
    /// </summary>
    public string AssessmentId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the risk level (0-1).
    /// </summary>
    public double RiskLevel { get; set; }

    /// <summary>
    /// Gets or sets the confidence level (0-1).
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets the potential impact description.
    /// </summary>
    public string ImpactDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the risk factors.
    /// </summary>
    public List<RiskFactor> RiskFactors { get; set; } = new();

    /// <summary>
    /// Gets or sets the mitigation strategies.
    /// </summary>
    public List<string> MitigationStrategies { get; set; } = new();

    /// <summary>
    /// Gets or sets when the assessment was performed.
    /// </summary>
    public DateTime AssessedAt { get; set; }

    /// <summary>
    /// Gets or sets the assessment metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the severity level.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets whether manual intervention is required.
    /// </summary>
    public bool RequiresManualIntervention { get; set; }
}

/// <summary>
/// Represents a risk factor in the assessment.
/// </summary>
public class RiskFactor
{
    /// <summary>
    /// Gets or sets the risk factor name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the risk factor description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the risk factor weight (0-1).
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Gets or sets the risk factor score (0-1).
    /// </summary>
    public double Score { get; set; }
} 