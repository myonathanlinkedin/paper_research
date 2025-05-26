using System;
using System.Collections.Generic;
using CodeSage.Core.Models.Error;
using CodeSage.Core.Remediation.Models.Validation;
using CodeSage.Core.Models;

namespace CodeSage.Core.Models;

public class RemediationResult
{
    public string RemediationId { get; set; } = Guid.NewGuid().ToString();
    public ErrorContext Context { get; set; } = new();
    public RemediationPlan Plan { get; set; } = new();
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public RemediationStatus Status { get; set; } = RemediationStatus.Pending;
    public string? Message { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string> CompletedSteps { get; set; } = new();
    public List<string> FailedSteps { get; set; } = new();
    public Dictionary<string, object> Metrics { get; set; } = new();
    public RemediationValidationResult Validation { get; set; } = new();
} 