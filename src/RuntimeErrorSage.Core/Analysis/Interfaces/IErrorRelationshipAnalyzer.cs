using System.Threading.Tasks;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Analysis.Interfaces
{
    /// <summary>
    /// Interface for analyzing relationships between errors.
    /// </summary>
    public interface IErrorRelationshipAnalyzer
    {
        /// <summary>
        /// Gets whether the analyzer is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the analyzer name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the analyzer version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the relationship type between two errors.
        /// </summary>
        /// <param name="sourceError">The source error.</param>
        /// <param name="targetError">The target error.</param>
        /// <returns>The error relationship.</returns>
        ErrorRelationship GetRelationshipType(RuntimeError sourceError, RuntimeError targetError);

        /// <summary>
        /// Finds related errors for a given error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The list of related errors.</returns>
        Task<List<RelatedError>> FindRelatedErrorsAsync(ErrorContext context);

        /// <summary>
        /// Analyzes the relationship between two errors.
        /// </summary>
        /// <param name="sourceError">The source error.</param>
        /// <param name="targetError">The target error.</param>
        /// <returns>The relationship strength (0-1).</returns>
        double AnalyzeRelationship(RuntimeError sourceError, RuntimeError targetError);

        /// <summary>
        /// Gets related errors for an error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>The list of related errors.</returns>
        Task<List<RelatedError>> GetRelatedErrorsAsync(RuntimeError error);

        /// <summary>
        /// Analyzes relationships between a set of errors.
        /// </summary>
        /// <param name="errors">The errors to analyze.</param>
        /// <returns>The list of error relationships.</returns>
        Task<List<ErrorRelationship>> AnalyzeRelationshipsAsync(List<RuntimeError> errors);

        /// <summary>
        /// Calculates the relationship strength between two errors.
        /// </summary>
        /// <param name="source">The source error.</param>
        /// <param name="target">The target error.</param>
        /// <returns>The relationship strength (0-1).</returns>
        Task<double> CalculateRelationshipStrengthAsync(RuntimeError source, RuntimeError target);
    }
} 
