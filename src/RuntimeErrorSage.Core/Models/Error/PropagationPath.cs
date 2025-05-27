using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Error;

/// <summary>
/// Represents a path of error propagation.
/// </summary>
public class PropagationPath
{
    /// <summary>
    /// Gets or sets the source component.
    /// </summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target component.
    /// </summary>
    public string Target { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the propagation time.
    /// </summary>
    public double PropagationTime { get; set; }

    /// <summary>
    /// Gets or sets the severity of the error.
    /// </summary>
    public ErrorSeverity Severity { get; set; }
} 