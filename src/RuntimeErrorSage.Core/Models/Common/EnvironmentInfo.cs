using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Common;

/// <summary>
/// Represents information about the runtime environment.
/// </summary>
public class EnvironmentInfo
{
    /// <summary>
    /// Gets or sets the environment name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the environment type.
    /// </summary>
    public EnvironmentType Type { get; set; }

    /// <summary>
    /// Gets or sets the operating system information.
    /// </summary>
    public OSInfo OS { get; set; }

    /// <summary>
    /// Gets or sets the runtime information.
    /// </summary>
    public RuntimeInfo Runtime { get; set; }

    /// <summary>
    /// Gets or sets the list of installed packages.
    /// </summary>
    public List<PackageInfo> Packages { get; set; } = new();

    /// <summary>
    /// Gets or sets the environment variables.
    /// </summary>
    public Dictionary<string, string> Variables { get; set; } = new();

    /// <summary>
    /// Gets or sets additional environment properties.
    /// </summary>
    public Dictionary<string, object> Properties { get; set; } = new();
}

/// <summary>
/// Represents the type of environment.
/// </summary>
public enum EnvironmentType
{
    Development,
    Testing,
    Staging,
    Production,
    Sandbox
}

/// <summary>
/// Represents operating system information.
/// </summary>
public class OSInfo
{
    /// <summary>
    /// Gets or sets the operating system name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the operating system version.
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets the operating system architecture.
    /// </summary>
    public string Architecture { get; set; }

    /// <summary>
    /// Gets or sets the operating system platform.
    /// </summary>
    public string Platform { get; set; }
}

/// <summary>
/// Represents runtime information.
/// </summary>
public class RuntimeInfo
{
    /// <summary>
    /// Gets or sets the runtime name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the runtime version.
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets the runtime framework.
    /// </summary>
    public string Framework { get; set; }

    /// <summary>
    /// Gets or sets whether the runtime is 64-bit.
    /// </summary>
    public bool Is64Bit { get; set; }
}

/// <summary>
/// Represents package information.
/// </summary>
public class PackageInfo
{
    /// <summary>
    /// Gets or sets the package name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the package version.
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets whether the package is a development dependency.
    /// </summary>
    public bool IsDevelopmentDependency { get; set; }

    /// <summary>
    /// Gets or sets additional package metadata.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new();
} 