using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Remediation;

/// <summary>
/// Represents the complexity assessment of a remediation action.
/// </summary>
public class RemediationComplexity
{
    /// <summary>
    /// Gets or sets the complexity score (0-100).
    /// </summary>
    public int ComplexityScore { get; set; }

    /// <summary>
    /// Gets or sets the factors contributing to complexity.
    /// </summary>
    public List<string> ComplexityFactors { get; set; } = new();

    /// <summary>
    /// Gets or sets the estimated time to implement in minutes.
    /// </summary>
    public int EstimatedTimeToImplement { get; set; }

    /// <summary>
    /// Gets or sets the required skill level (1-5).
    /// </summary>
    public int RequiredSkillLevel { get; set; }

    /// <summary>
    /// Gets or sets the number of steps involved.
    /// </summary>
    public int StepCount { get; set; }

    /// <summary>
    /// Gets or sets the number of dependencies.
    /// </summary>
    public int DependencyCount { get; set; }

    /// <summary>
    /// Gets or sets the risk level associated with complexity.
    /// </summary>
    public int RiskLevel { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the assessment.
    /// </summary>
    public DateTime AssessmentTimestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets additional complexity metrics.
    /// </summary>
    public Dictionary<string, double> ComplexityMetrics { get; set; } = new();

    /// <summary>
    /// Gets or sets notes about the complexity assessment.
    /// </summary>
    public string AssessmentNotes { get; set; } = string.Empty;
} 
