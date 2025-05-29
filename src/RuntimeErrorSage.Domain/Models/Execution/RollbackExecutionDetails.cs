using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Execution;

/// <summary>
/// Contains detailed information about a rollback execution.
/// </summary>
public class RollbackExecutionDetails
{
    /// <summary>
    /// Gets or sets the unique identifier for this rollback execution.
    /// </summary>
    public string RollbackId { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the status of the rollback.
    /// </summary>
    public RemediationStatusEnum Status { get; }

    /// <summary>
    /// Gets or sets the start time of the rollback.
    /// </summary>
    public DateTime StartTime { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the end time of the rollback.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Gets or sets whether the rollback was successful.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Gets or sets the error message if the rollback failed.
    /// </summary>
    public string ErrorMessage { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of steps executed during rollback.
    /// </summary>
    public IReadOnlyCollection<ExecutedSteps> ExecutedSteps { get; } = new();

    /// <summary>
    /// Gets or sets the list of failed steps.
    /// </summary>
    public IReadOnlyCollection<FailedSteps> FailedSteps { get; } = new();

    /// <summary>
    /// Gets or sets the correlation ID.
    /// </summary>
    public string CorrelationId { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the error ID this rollback is associated with.
    /// </summary>
    public string ErrorId { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the remediation ID this rollback is associated with.
    /// </summary>
    public string RemediationId { get; } = string.Empty;

    /// <summary>
    /// Gets or sets additional metadata about the rollback.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 






