using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Graph;

/// <summary>
/// Represents the result of a root cause analysis.
/// </summary>
public class RootCauseAnalysisResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the analysis.
    /// </summary>
    public string AnalysisId { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the error identifier.
    /// </summary>
    public string ErrorId { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the root cause node.
    /// </summary>
    public GraphNode RootCauseNode { get; }

    /// <summary>
    /// Gets or sets the path from the root cause to the error.
    /// </summary>
    public GraphPath PathToError { get; }

    /// <summary>
    /// Gets or sets the confidence level of the analysis (0.0 to 1.0).
    /// </summary>
    public double Confidence { get; }

    /// <summary>
    /// Gets or sets the status of the analysis.
    /// </summary>
    public AnalysisStatus Status { get; }

    /// <summary>
    /// Gets or sets the timestamp when the analysis was performed.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the description of the root cause.
    /// </summary>
    public string Description { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the alternative root causes.
    /// </summary>
    public IReadOnlyCollection<AlternativeRootCauses> AlternativeRootCauses { get; } = new();

    /// <summary>
    /// Gets or sets the contributing factors.
    /// </summary>
    public IReadOnlyCollection<ContributingFactors> ContributingFactors { get; } = new();
} 






