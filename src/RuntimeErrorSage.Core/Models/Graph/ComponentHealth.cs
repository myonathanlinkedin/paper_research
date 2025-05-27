using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents the health status of a component.
/// </summary>
public class ComponentHealth
{
    /// <summary>
    /// Gets or sets the name of the component.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the health status.
    /// </summary>
    public HealthStatusEnum Status { get; set; }

    /// <summary>
    /// Gets or sets the metrics associated with the component.
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; } = new();
} 