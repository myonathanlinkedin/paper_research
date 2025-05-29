using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Tests.TestSuite.Models;

/// <summary>
/// Results from comparing baseline and experimental tests
/// </summary>
public class ComparisonResults
{
    /// <summary>
    /// Unique identifier for the comparison
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Name of the comparison
    /// </summary>
    public string Name { get; } = string.Empty;

    /// <summary>
    /// Baseline test results
    /// </summary>
    public BaselineResult Baseline { get; } = new();

    /// <summary>
    /// Experimental test results
    /// </summary>
    public BaselineResult Experimental { get; } = new();

    /// <summary>
    /// Statistical significance of the comparison
    /// </summary>
    public double StatisticalSignificance { get; }

    /// <summary>
    /// Whether the experimental results are significantly better
    /// </summary>
    public bool IsSignificantlyBetter { get; }

    /// <summary>
    /// Additional metadata for the comparison
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 






