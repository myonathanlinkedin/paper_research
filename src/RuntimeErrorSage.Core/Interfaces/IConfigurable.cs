using RuntimeErrorSage.Core.Options;

namespace RuntimeErrorSage.Core.Interfaces;

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