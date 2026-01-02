using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Tests.TestSuite.Models;

/// <summary>
/// Represents the expected result for a test scenario.
/// </summary>
public class ExpectedResult
{
    /// <summary>
    /// Gets or sets the expected accuracy (0.0 to 1.0).
    /// </summary>
    public double Accuracy { get; set; }

    /// <summary>
    /// Gets or sets the expected latency in milliseconds.
    /// </summary>
    public double Latency { get; set; }

    /// <summary>
    /// Gets or sets the expected severity.
    /// </summary>
    public ErrorSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets whether auto-remediation is expected.
    /// </summary>
    public bool CanAutoRemediate { get; set; }

    /// <summary>
    /// Gets or sets the expected root cause.
    /// </summary>
    public string RootCause { get; set; } = string.Empty;
}







