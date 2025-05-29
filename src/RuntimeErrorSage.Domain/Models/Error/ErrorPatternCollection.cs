using System.Collections.ObjectModel;
using RuntimeErrorSage.Domain.Enums;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Error;

/// <summary>
/// Represents a collection of error patterns.
/// </summary>
public class ErrorPatternCollection
{
    /// <summary>
    /// Gets or sets the collection of error patterns.
    /// </summary>
    public IReadOnlyCollection<Patterns> Patterns { get; } = new();

    /// <summary>
    /// Gets or sets the total count.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Gets or sets the match scores.
    /// </summary>
    public Dictionary<string, double> MatchScores { get; set; } = new();
} 






