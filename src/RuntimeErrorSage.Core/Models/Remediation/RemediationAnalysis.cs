using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Models.Remediation;

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
    public ErrorContext Context { get; set; }

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
    public RemediationSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the analysis priority.
    /// </summary>
    public RemediationPriority Priority { get; set; }

    /// <summary>
    /// Gets or sets the analysis impact.
    /// </summary>
    public RemediationImpact Impact { get; set; }

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
    public RemediationStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the error message if analysis failed.
    /// </summary>
    public string ErrorMessage { get; set; }
}

/// <summary>
/// Specifies the remediation severity.
/// </summary>
public enum RemediationSeverity
{
    /// <summary>
    /// Low severity.
    /// </summary>
    Low,

    /// <summary>
    /// Medium severity.
    /// </summary>
    Medium,

    /// <summary>
    /// High severity.
    /// </summary>
    High,

    /// <summary>
    /// Critical severity.
    /// </summary>
    Critical
}

/// <summary>
/// Specifies the remediation priority.
/// </summary>
public enum RemediationPriority
{
    /// <summary>
    /// Low priority.
    /// </summary>
    Low,

    /// <summary>
    /// Medium priority.
    /// </summary>
    Medium,

    /// <summary>
    /// High priority.
    /// </summary>
    High,

    /// <summary>
    /// Critical priority.
    /// </summary>
    Critical
} 