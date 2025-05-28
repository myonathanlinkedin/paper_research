using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Graph;

/// <summary>
/// Represents dependency information.
/// </summary>
public class DependencyInfo
{
    /// <summary>
    /// Gets or sets the dependency identifier.
    /// </summary>
    public string DependencyId { get; set; }

    /// <summary>
    /// Gets or sets the dependency type.
    /// </summary>
    public DependencyType Type { get; set; }

    /// <summary>
    /// Gets or sets the dependency strength.
    /// </summary>
    public double Strength { get; set; }

    /// <summary>
    /// Gets or sets whether the dependency is critical.
    /// </summary>
    public bool IsCritical { get; set; }
} 
