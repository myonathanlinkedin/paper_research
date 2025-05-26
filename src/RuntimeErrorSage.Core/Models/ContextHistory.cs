using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Models;

public class ContextHistory
{
    public string ContextId { get; set; } = string.Empty;
    public List<ErrorContext> ErrorContexts { get; set; } = new();
    public TimeRange TimeRange { get; set; } = new();
    public Dictionary<string, object> Metadata { get; set; } = new();
    public int ErrorCount => ErrorContexts.Count;
    public DateTime LastUpdated { get; set; }

    public ContextHistory()
    {
        LastUpdated = DateTime.UtcNow;
    }

    public void AddContext(ErrorContext context)
    {
        ErrorContexts.Add(context);
        LastUpdated = DateTime.UtcNow;
    }

    public void Clear()
    {
        ErrorContexts.Clear();
        LastUpdated = DateTime.UtcNow;
    }
} 
