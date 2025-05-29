using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Remediation;

/// <summary>
/// Represents a registry of remediation strategies.
/// </summary>
public class RemediationRegistry
{
    /// <summary>
    /// Gets or sets the unique identifier of the registry.
    /// </summary>
    public string RegistryId { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the registry.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets the description of the registry.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets or sets the registered strategies.
    /// </summary>
    public IReadOnlyCollection<Strategies> Strategies { get; } = new();

    /// <summary>
    /// Gets or sets the registry metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
} 






