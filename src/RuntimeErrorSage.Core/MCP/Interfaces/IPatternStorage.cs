using System;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.MCP.Interfaces
{
    /// <summary>
    /// Interface for storing and retrieving patterns.
    /// </summary>
    public interface IPatternStorage : IDisposable
    {
        /// <summary>
        /// Saves a pattern to storage.
        /// </summary>
        /// <param name="pattern">The pattern to save.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SavePatternAsync(ErrorPatternEnum pattern);

        /// <summary>
        /// Retrieves a pattern by its ID.
        /// </summary>
        /// <param name="patternId">The ID of the pattern to retrieve.</param>
        /// <returns>The pattern with the specified ID, or null if not found.</returns>
        Task<ErrorPatternEnum> GetPatternByIdAsync(string patternId);

        /// <summary>
        /// Searches for patterns matching the specified criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>A list of patterns matching the criteria.</returns>
        Task<List<ErrorPatternEnum>> SearchPatternsAsync(PatternSearchCriteria searchCriteria);

        /// <summary>
        /// Gets all patterns.
        /// </summary>
        /// <returns>A list of all patterns.</returns>
        Task<List<ErrorPatternEnum>> GetAllPatternsAsync();

        /// <summary>
        /// Updates a pattern.
        /// </summary>
        /// <param name="pattern">The pattern to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdatePatternAsync(ErrorPatternEnum pattern);

        /// <summary>
        /// Deletes a pattern.
        /// </summary>
        /// <param name="patternId">The ID of the pattern to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeletePatternAsync(string patternId);

        /// <summary>
        /// Gets the count of patterns.
        /// </summary>
        /// <returns>The count of patterns.</returns>
        Task<int> GetPatternCountAsync();

        /// <summary>
        /// Gets patterns by tag.
        /// </summary>
        /// <param name="tag">The tag to search for.</param>
        /// <returns>A list of patterns with the specified tag.</returns>
        Task<List<ErrorPatternEnum>> GetPatternsByTagAsync(string tag);

        /// <summary>
        /// Gets patterns by category.
        /// </summary>
        /// <param name="category">The category to search for.</param>
        /// <returns>A list of patterns in the specified category.</returns>
        Task<List<ErrorPatternEnum>> GetPatternsByCategoryAsync(string category);

        /// <summary>
        /// Validates the connection to the pattern storage.
        /// </summary>
        /// <returns>True if the connection is valid, otherwise false.</returns>
        Task<bool> ValidateConnectionAsync();

        /// <summary>
        /// Saves a list of patterns to storage.
        /// </summary>
        /// <param name="patterns">The patterns to save.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SavePatternsAsync(List<ErrorPattern> patterns);

        /// <summary>
        /// Gets patterns for a specific service.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <returns>A list of error patterns.</returns>
        Task<List<ErrorPattern>> GetPatternsAsync(string serviceName);
    }
} 