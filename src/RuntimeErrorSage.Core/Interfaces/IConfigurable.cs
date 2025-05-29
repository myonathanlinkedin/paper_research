using RuntimeErrorSage.Model.Options;

namespace RuntimeErrorSage.Model.Interfaces;

/// <summary>
/// Interface for configurable components
/// </summary>
public interface IConfigurable
{
    /// <summary>
    /// Configures the component with the specified options
    /// </summary>
    /// <param name="options">The configuration options</param>
    void Configure(RuntimeErrorSageOptions options);
} 