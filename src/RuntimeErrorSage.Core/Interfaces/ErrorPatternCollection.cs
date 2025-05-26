using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents a collection of error patterns.
/// </summary>
public class ErrorPatternCollection
{
    public List<ErrorPattern> Patterns { get; set; } = new();
    public int TotalCount { get; set; }
    public Dictionary<string, double> MatchScores { get; set; } = new();
} 