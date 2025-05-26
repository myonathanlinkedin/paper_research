using System.Collections.Generic;

namespace CodeSage.Core.Remediation.Models
{
    public class MetricsValidation
    {
        public bool IsWithinThresholds { get; set; }
        public Dictionary<string, object> Details { get; set; } = new();
    }
} 