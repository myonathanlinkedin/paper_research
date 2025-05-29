using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Model.Models.Health;

/// <summary>
/// Represents a health issue in the system.
/// </summary>
public class HealthIssue
{
    /// <summary>
    /// Gets or sets the issue code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the issue message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the issue severity.
    /// </summary>
    public SeverityLevel Severity { get; set; }

    /// <summary>
    /// Gets or sets additional details about the issue.
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new();
} 
