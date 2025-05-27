using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents a pattern of errors in the system.
/// </summary>
public class ErrorPattern
{
    /// <summary>
    /// Gets or sets the unique identifier for this pattern.
    /// </summary>
    public string PatternId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the pattern name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the pattern description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error severity.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the pattern frequency.
    /// </summary>
    public int Frequency { get; set; }

    /// <summary>
    /// Gets or sets the pattern tags.
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Gets or sets the pattern category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the pattern creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the pattern last update timestamp.
    /// </summary>
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the pattern metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets whether the pattern is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the pattern confidence score (0-1).
    /// </summary>
    public double ConfidenceScore { get; set; }
} 