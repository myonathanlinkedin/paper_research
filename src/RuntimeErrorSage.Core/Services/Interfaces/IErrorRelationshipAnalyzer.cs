using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;
using RelatedErrorModel = RuntimeErrorSage.Core.Models.Error.RelatedError;

namespace RuntimeErrorSage.Core.Services.Interfaces;

/// <summary>
/// Service for analyzing relationships between errors.
/// </summary>
public interface IErrorRelationshipAnalyzer
{
    /// <summary>
    /// Finds errors related to the given error context.
    /// </summary>
    /// <param name="context">The error context</param>
    /// <param name="graph">The dependency graph</param>
    /// <returns>A collection of related errors</returns>
    Task<IEnumerable<RelatedErrorModel>> FindRelatedErrorsAsync(ErrorContext context, DependencyGraph graph);

    /// <summary>
    /// Analyzes the relationship between two errors.
    /// </summary>
    /// <param name="error1">The first error</param>
    /// <param name="error2">The second error</param>
    /// <returns>The relationship type and strength</returns>
    Task<ErrorRelationship> AnalyzeRelationshipAsync(ErrorContext error1, ErrorContext error2);

    /// <summary>
    /// Validates the relationship analyzer configuration.
    /// </summary>
    /// <returns>True if the configuration is valid, false otherwise</returns>
    Task<bool> ValidateConfigurationAsync();

    /// <summary>
    /// Analyzes the relationships between errors in the given context.
    /// </summary>
    /// <param name="context">The error context to analyze.</param>
    /// <returns>A list of related errors.</returns>
    Task<IEnumerable<RelatedErrorModel>> AnalyzeRelationshipsAsync(ErrorContext context);

    /// <summary>
    /// Gets the error relationship type between two errors.
    /// </summary>
    /// <param name="source">The source error.</param>
    /// <param name="target">The target error.</param>
    /// <returns>The relationship type between the errors.</returns>
    ErrorRelationship GetRelationshipType(RuntimeError source, RuntimeError target);
} 