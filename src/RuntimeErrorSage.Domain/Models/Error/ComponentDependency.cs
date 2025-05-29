namespace RuntimeErrorSage.Model.Models.Error;

/// <summary>
/// Represents a component dependency in the error context.
/// </summary>
public class ComponentDependency
{
    /// <summary>
    /// Gets or sets the source component.
    /// </summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target component.
    /// </summary>
    public string Target { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of dependency.
    /// </summary>
    public string DependencyType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the strength of the dependency.
    /// </summary>
    public double Strength { get; set; }
} 
