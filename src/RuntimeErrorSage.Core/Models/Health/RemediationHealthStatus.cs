using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Models.Health;

/// <summary>
/// Represents the health status of a remediation system.
/// </summary>
public class RemediationHealthStatus : HealthStatusInfo
{
    /// <summary>
    /// Gets or sets the health score between 0.0 and 1.0.
    /// </summary>
    public double HealthScore { get; set; }

    /// <summary>
    /// Gets or sets the service name.
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    /// Gets or sets the list of health issues.
    /// </summary>
    public List<HealthIssue> Issues { get; set; } = new();
} 