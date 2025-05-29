using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.MCP
{
    public class ContextAnalysisResult
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ContextId { get; set; } = string.Empty;
        public DateTime AnalyzedAt { get; set; } = DateTime.UtcNow;
        public Dictionary<string, object> Analysis { get; set; } = new();
        public Dictionary<string, object> Metrics { get; set; } = new();
        public Dictionary<string, object> Health { get; set; } = new();
        public Dictionary<string, object> Validation { get; set; } = new();
        public Dictionary<string, object> Remediation { get; set; } = new();
        public List<string> Issues { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
        public string Version { get; set; } = "1.0.0";
        public bool IsValid { get; set; } = true;
        public string Status { get; set; } = "Success";
        public Dictionary<string, object> Configuration { get; set; } = new();
        public Dictionary<string, object> State { get; set; } = new();
    }
} 
