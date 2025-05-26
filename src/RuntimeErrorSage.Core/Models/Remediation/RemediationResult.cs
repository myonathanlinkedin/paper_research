using System;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the result of a remediation action.
    /// </summary>
    public class RemediationResult
    {
        public string RemediationId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 