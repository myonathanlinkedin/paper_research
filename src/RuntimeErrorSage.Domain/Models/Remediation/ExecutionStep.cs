using System;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation;

/// <summary>
/// Represents an execution step for a remediation action.
/// </summary>
public class ExecutionStep
{
    /// <summary>
    /// Gets or sets the step identifier.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the step name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the step description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the step order.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets whether the step is required.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets the step status.
    /// </summary>
    public ActionStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the step start time.
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Gets or sets the step end time.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the step error message.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the step metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 
