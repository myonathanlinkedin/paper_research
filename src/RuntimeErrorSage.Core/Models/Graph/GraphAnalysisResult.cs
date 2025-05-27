using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.LLM;

namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents the result of a graph analysis operation.
/// </summary>
public class GraphAnalysisResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the analysis.
    /// </summary>
    public string AnalysisId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the dependency graph used for analysis.
    /// </summary>
    public DependencyGraph DependencyGraph { get; set; }

    /// <summary>
    /// Gets or sets the impact analysis results.
    /// </summary>
    public List<ImpactAnalysisResult> ImpactResults { get; set; } = new();

    /// <summary>
    /// Gets or sets the related errors found during analysis.
    /// </summary>
    public List<RelatedError> RelatedErrors { get; set; } = new();

    /// <summary>
    /// Gets or sets the analysis start time.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the analysis end time.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the analysis duration in milliseconds.
    /// </summary>
    public double DurationMs => EndTime.HasValue ? (EndTime.Value - StartTime).TotalMilliseconds : 0;

    /// <summary>
    /// Gets or sets the analysis status.
    /// </summary>
    public AnalysisStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the analysis error message if any.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the analysis metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the correlation ID.
    /// </summary>
    public string CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the analysis.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the analysis metrics.
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; } = new();
}