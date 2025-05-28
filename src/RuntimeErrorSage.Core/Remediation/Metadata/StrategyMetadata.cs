namespace RuntimeErrorSage.Core.Remediation.Metadata;

/// <summary>
/// Metadata for a remediation strategy, including version, description, author, and requirements.
/// </summary>
public class StrategyMetadata
{
    /// <summary>Version of the strategy.</summary>
    public string Version { get; set; } = string.Empty;
    /// <summary>Description of the strategy.</summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>Author of the strategy.</summary>
    public string Author { get; set; } = string.Empty;
    /// <summary>Date the strategy was created.</summary>
    public DateTime CreationDate { get; set; }
    /// <summary>Date the strategy was last modified.</summary>
    public DateTime LastModifiedDate { get; set; }
    /// <summary>Dependencies required by the strategy.</summary>
    public Dictionary<string, string> Dependencies { get; set; } = new();
    /// <summary>Requirements for the strategy.</summary>
    public Dictionary<string, string> Requirements { get; set; } = new();
    /// <summary>Indicates if the strategy is deprecated.</summary>
    public bool IsDeprecated { get; set; }
    /// <summary>Reason for deprecation, if applicable.</summary>
    public string? DeprecationReason { get; set; }
    /// <summary>Replacement strategy, if deprecated.</summary>
    public string? ReplacementStrategy { get; set; }
} 
