using System;
using System.Collections.Generic;

namespace CodeSage.Core.Models;

public class ModelInfo
{
    public string ModelId { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string ModelVersion { get; set; } = string.Empty;
    public string ModelType { get; set; } = string.Empty;
    public Dictionary<string, object> Parameters { get; set; } = new();
    public Dictionary<string, object> Capabilities { get; set; } = new();
    public DateTime LastUpdated { get; set; }
    public bool IsReady { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
} 