using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Health;

/// <summary>
/// Represents the health status of a service or component.
/// </summary>
public class ComponentHealth
{
    public string ServiceName { get; set; } = string.Empty;
    public bool IsHealthy { get; set; }
    public double HealthScore { get; set; }
    public Dictionary<string, double> Metrics { get; set; } = new();
    public HealthPrediction? Prediction { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public HealthStatus Status { get; set; }
} 