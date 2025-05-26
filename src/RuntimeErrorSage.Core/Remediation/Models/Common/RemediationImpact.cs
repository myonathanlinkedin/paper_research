using System;

namespace RuntimeErrorSage.Core.Remediation.Models.Common
{
    public class RemediationImpact
    {
        public string RemediationId { get; set; } = string.Empty;
        public ImpactScope Scope { get; set; }
        public ImpactSeverity Severity { get; set; }
        public List<string> AffectedComponents { get; set; } = new();
        public Dictionary<string, object> ImpactDetails { get; set; } = new();
        public DateTime AssessmentTime { get; set; }
        public string? AssessmentNotes { get; set; }
    }

    public enum ImpactScope
    {
        Local,
        Component,
        Service,
        System,
        Global
    }

    public enum ImpactSeverity
    {
        None,
        Low,
        Medium,
        High,
        Critical
    }
} 
