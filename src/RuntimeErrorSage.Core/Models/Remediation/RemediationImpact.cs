using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Represents the impact of a remediation operation.
/// </summary>
public class RemediationImpact
{
    /// <summary>
    /// Gets or sets the scope of the impact.
    /// </summary>
    public RemediationActionImpactScope Scope { get; set; }

    /// <summary>
    /// Gets or sets the severity of the impact.
    /// </summary>
    public RemediationActionSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the affected components.
    /// </summary>
    public List<string> AffectedComponents { get; set; } = new();

    /// <summary>
    /// Gets or sets the impact metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 