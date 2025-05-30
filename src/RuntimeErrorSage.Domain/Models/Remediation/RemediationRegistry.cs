using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Remediation;

/// <summary>
/// Represents a registry of remediation strategies.
/// </summary>
public class RemediationRegistry
{
    /// <summary>
    /// Gets or sets the unique identifier of the registry.
    /// </summary>
    public string RegistryId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the registry.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the registry.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the registered strategies.
    /// </summary>
    public List<RemediationStrategyModel> Strategies { get; set; } = new();

    /// <summary>
    /// Gets or sets the registry metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 
