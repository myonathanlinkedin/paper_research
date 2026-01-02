using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Error;

/// <summary>
/// Represents a pattern of errors that can be used for analysis and remediation.
/// </summary>
public class ErrorPattern
{
    /// <summary>
    /// Gets or sets the unique identifier for this pattern.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the service where this pattern occurs.
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    /// Gets or sets the type of error this pattern represents.
    /// </summary>
    public string ErrorType { get; set; }

    /// <summary>
    /// Gets or sets the name of the operation where this pattern occurs.
    /// </summary>
    public string OperationName { get; set; }

    /// <summary>
    /// Gets or sets when this pattern was first observed.
    /// </summary>
    public DateTime FirstOccurrence { get; set; }

    /// <summary>
    /// Gets or sets when this pattern was last updated.
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// Gets or sets the number of times this pattern has been observed.
    /// </summary>
    public int OccurrenceCount { get; set; }

    /// <summary>
    /// Gets or sets the context in which this pattern occurs.
    /// </summary>
    public Dictionary<string, object> Context { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets the remediation strategies for this pattern.
    /// </summary>
    public List<string> RemediationStrategies { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets additional metadata about this pattern.
    /// </summary>
    public Dictionary<string, object> PatternMetadata { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets whether this pattern is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the analysis result for this pattern.
    /// </summary>
    public ErrorAnalysisResult Analysis { get; set; }

    /// <summary>
    /// Gets or sets additional notes about this pattern.
    /// </summary>
    public string Notes { get; set; }

    /// <summary>
    /// Gets or sets the type of this pattern.
    /// </summary>
    public string PatternType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets when this pattern was last observed.
    /// </summary>
    public DateTime LastOccurrence { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the confidence level of this pattern (0-1).
    /// </summary>
    public double Confidence { get; set; } = 0.0;
} 
