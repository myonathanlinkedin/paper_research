using System;
using System.Collections.Generic;
using CodeSage.Core.Remediation.Models.Common;

namespace CodeSage.Core.Remediation.Models.Validation
{
    public class RemediationValidationResult
    {
        public string RemediationId { get; set; } = string.Empty;
        public bool IsValid { get; set; }
        public List<ValidationIssue> Issues { get; set; } = new();
        public List<ValidationError> Errors { get; set; } = new();
        public List<ValidationWarning> Warnings { get; set; } = new();
        public RemediationImpact Impact { get; set; } = new();
        public Dictionary<string, object> ValidationDetails { get; set; } = new();
        public DateTime ValidationTime { get; set; } = DateTime.UtcNow;
        public string? ValidationNotes { get; set; }
        public long ValidationDurationMs { get; set; }
        public bool RequiresApproval { get; set; }
        public string? ApprovalReason { get; set; }
    }

    public class ValidationIssue
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public ValidationSeverity Severity { get; set; }
        public string? Component { get; set; }
        public Dictionary<string, object> Details { get; set; } = new();
    }

    public class ValidationError
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Component { get; set; }
        public Dictionary<string, object> Details { get; set; } = new();
    }

    public class ValidationWarning
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Component { get; set; }
        public Dictionary<string, object> Details { get; set; } = new();
    }

    public enum ValidationSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }
} 