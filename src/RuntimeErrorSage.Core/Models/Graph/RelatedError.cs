using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents a related error in the dependency graph.
/// </summary>
public class RelatedError
{
    /// <summary>
    /// Gets or sets the unique identifier of the related error.
    /// </summary>
    public string ErrorId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the error type.
    /// </summary>
    public string ErrorType { get; set; }

    /// <summary>
    /// Gets or sets the component where the error occurred.
    /// </summary>
    public string ComponentId { get; set; }

    /// <summary>
    /// Gets or sets the relationship type with the original error.
    /// </summary>
    public RelationshipType RelationType { get; set; }

    /// <summary>
    /// Gets or sets the relationship strength (0-1).
    /// </summary>
    public double RelationshipStrength { get; set; }

    /// <summary>
    /// Gets or sets the confidence level of the relationship (0-1).
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the error occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the error severity.
    /// </summary>
    public ImpactSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets whether the error is still active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the error metrics.
    /// </summary>
    public Dictionary<string, double> Metrics { get; set; } = new();

    /// <summary>
    /// Gets or sets the error metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the stack trace if available.
    /// </summary>
    public string StackTrace { get; set; }

    /// <summary>
    /// Gets or sets the error context data.
    /// </summary>
    public Dictionary<string, string> Context { get; set; } = new();
} 