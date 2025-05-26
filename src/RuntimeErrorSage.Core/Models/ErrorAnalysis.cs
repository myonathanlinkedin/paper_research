using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models
{
    public class ErrorAnalysis
    {
        public string RootCause { get; set; }
        public List<string> RemediationSteps { get; set; } = new List<string>();
        public float Confidence { get; set; }
    }
} 
