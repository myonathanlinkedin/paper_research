using System.Threading.Tasks;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.Graph;

namespace RuntimeErrorSage.Model.Services.Interfaces;

/// <summary>
/// Service for building dependency graphs from error contexts.
/// </summary>
public interface IGraphBuilder
{
    /// <summary>
    /// Builds a dependency graph from an error context.
    /// </summary>
    /// <param name="context">The error context</param>
    /// <returns>The constructed dependency graph</returns>
    Task<DependencyGraph> BuildGraphAsync(ErrorContext context);

    /// <summary>
    /// Validates the graph builder configuration.
    /// </summary>
    /// <returns>True if the configuration is valid, false otherwise</returns>
    Task<bool> ValidateConfigurationAsync();
} 
