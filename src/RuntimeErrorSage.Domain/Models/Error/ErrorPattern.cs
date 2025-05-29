using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Error;

/// <summary>
/// Represents a pattern of errors that can be used for analysis and remediation.
/// </summary>
public class ErrorPattern
{
    /// <summary>
    /// Gets or sets the unique identifier for this pattern.
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the service where this pattern occurs.
    /// </summary>
    public string ServiceName { get; }

    /// <summary>
    /// Gets or sets the type of error this pattern represents.
    /// </summary>
    public string ErrorType { get; }

    /// <summary>
    /// Gets or sets the name of the operation where this pattern occurs.
    /// </summary>
    public string OperationName { get; }

    /// <summary>
    /// Gets or sets when this pattern was first observed.
    /// </summary>
    public DateTime FirstOccurrence { get; }

    /// <summary>
    /// Gets or sets when this pattern was last updated.
    /// </summary>
    public DateTime LastUpdated { get; }

    /// <summary>
    /// Gets or sets the number of times this pattern has been observed.
    /// </summary>
    public int OccurrenceCount { get; }

    /// <summary>
    /// Gets or sets the context in which this pattern occurs.
    /// </summary>
    public Dictionary<string, object> Context { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets the remediation strategies for this pattern.
    /// </summary>
    public IReadOnlyCollection<RemediationStrategies> RemediationStrategies { get; } = new Collection<string>();

    /// <summary>
    /// Gets or sets additional metadata about this pattern.
    /// </summary>
    public Dictionary<string, object> PatternMetadata { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets whether this pattern is currently active.
    /// </summary>
    public bool IsActive { get; } = true;

    /// <summary>
    /// Gets or sets the analysis result for this pattern.
    /// </summary>
    public ErrorAnalysisResult Analysis { get; }

    /// <summary>
    /// Gets or sets additional notes about this pattern.
    /// </summary>
    public string Notes { get; }
} 






