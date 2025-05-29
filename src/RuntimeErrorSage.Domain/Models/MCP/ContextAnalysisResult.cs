using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.MCP
{
    public class ContextAnalysisResult
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string ContextId { get; } = string.Empty;
        public DateTime AnalyzedAt { get; } = DateTime.UtcNow;
        public Dictionary<string, object> Analysis { get; set; } = new();
        public Dictionary<string, object> Metrics { get; set; } = new();
        public Dictionary<string, object> Health { get; set; } = new();
        public Dictionary<string, object> Validation { get; set; } = new();
        public Dictionary<string, object> Remediation { get; set; } = new();
        public IReadOnlyCollection<Issues> Issues { get; } = new();
        public IReadOnlyCollection<Recommendations> Recommendations { get; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
        public string Version { get; } = "1.0.0";
        public bool IsValid { get; } = true;
        public string Status { get; } = "Success";
        public Dictionary<string, object> Configuration { get; set; } = new();
        public Dictionary<string, object> State { get; set; } = new();
    }
} 





