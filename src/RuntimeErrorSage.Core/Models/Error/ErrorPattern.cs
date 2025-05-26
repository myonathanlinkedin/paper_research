using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Error
{
    public class ErrorPattern
    {
        public string PatternId { get; set; } = Guid.NewGuid().ToString();
        public string ServiceName { get; set; } = string.Empty;
        public string ErrorType { get; set; } = string.Empty;
        public string OperationName { get; set; } = string.Empty;
        public DateTime FirstOccurrence { get; set; }
        public DateTime LastOccurrence { get; set; }
        public int OccurrenceCount { get; set; }
        public ErrorContext Context { get; set; } = new();
        public List<string> RemediationStrategies { get; set; } = new();
        public Dictionary<string, object> PatternMetadata { get; set; } = new();
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
    }
} 
