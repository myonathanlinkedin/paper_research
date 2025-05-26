using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.MCP
{
    public class RuntimeContext
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Dictionary<string, object> Metadata { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdated { get; set; }
        public string Version { get; set; } = "1.0.0";
        public bool IsActive { get; set; } = true;
        public Dictionary<string, string> Tags { get; set; } = new();
        public Dictionary<string, object> Configuration { get; set; } = new();
        public List<string> Dependencies { get; set; } = new();
        public Dictionary<string, object> State { get; set; } = new();
        public Dictionary<string, object> Metrics { get; set; } = new();
        public Dictionary<string, object> Health { get; set; } = new();
        public Dictionary<string, object> Validation { get; set; } = new();
        public Dictionary<string, object> Remediation { get; set; } = new();
    }
} 