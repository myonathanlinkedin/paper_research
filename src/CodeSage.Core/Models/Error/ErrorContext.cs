using System;
using System.Collections.Generic;

namespace CodeSage.Core.Models.Error
{
    public class ErrorContext
    {
        public string ServiceName { get; set; } = string.Empty;
        public string OperationName { get; set; } = string.Empty;
        public string CorrelationId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public Exception? Exception { get; set; }
        public Dictionary<string, object> AdditionalContext { get; set; } = new();
        public ErrorAnalysisResult? Analysis { get; set; }
        public ErrorSeverity Severity { get; set; }
        public string? UserId { get; set; }
        public string? Environment { get; set; }
        public Dictionary<string, string> Tags { get; set; } = new();
    }
} 