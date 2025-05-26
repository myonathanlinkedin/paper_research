using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Services.Interfaces;

/// <summary>
/// Service for enriching error contexts with additional runtime information.
/// </summary>
public interface IContextEnrichmentService
{
    /// <summary>
    /// Enriches an error context with additional runtime information.
    /// </summary>
    /// <param name="context">The base error context to enrich</param>
    /// <returns>The enriched error context</returns>
    Task<ErrorContext> EnrichContextAsync(ErrorContext context);

    /// <summary>
    /// Validates that an error context contains all required information.
    /// </summary>
    /// <param name="context">The error context to validate</param>
    /// <returns>True if the context is valid, false otherwise</returns>
    Task<bool> ValidateContextAsync(ErrorContext context);

    /// <summary>
    /// Gets the current runtime context information.
    /// </summary>
    /// <returns>A dictionary containing runtime context information</returns>
    Task<Dictionary<string, object>> GetRuntimeContextAsync();
} 