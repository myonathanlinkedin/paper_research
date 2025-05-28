using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.Storage.Interfaces
{
    /// <summary>
    /// Defines the contract for storing and retrieving error patterns
    /// </summary>
    public interface IPatternStorage
    {
        /// <summary>
        /// Stores a pattern with the specified key
        /// </summary>
        /// <param name="key">The key to store the pattern under</param>
        /// <param name="pattern">The pattern to store</param>
        /// <param name="expiration">Optional expiration time for the pattern</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task StorePatternAsync(string key, string pattern, TimeSpan? expiration = null);

        /// <summary>
        /// Retrieves a pattern by its key
        /// </summary>
        /// <param name="key">The key of the pattern to retrieve</param>
        /// <returns>A task containing the pattern if found, null otherwise</returns>
        Task<string> GetPatternAsync(string key);

        /// <summary>
        /// Removes a pattern by its key
        /// </summary>
        /// <param name="key">The key of the pattern to remove</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task RemovePatternAsync(string key);

        /// <summary>
        /// Checks if a pattern exists with the specified key
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>A task containing true if the pattern exists, false otherwise</returns>
        Task<bool> PatternExistsAsync(string key);

        /// <summary>
        /// Gets all patterns matching the specified pattern
        /// </summary>
        /// <param name="pattern">The pattern to match against</param>
        /// <returns>A task containing a dictionary of matching patterns</returns>
        Task<Dictionary<string, string>> GetPatternsAsync(string pattern);

        /// <summary>
        /// Updates the expiration time for a pattern
        /// </summary>
        /// <param name="key">The key of the pattern to update</param>
        /// <param name="expiration">The new expiration time</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task UpdateExpirationAsync(string key, TimeSpan expiration);

        /// <summary>
        /// Clears all patterns from storage
        /// </summary>
        /// <returns>A task representing the asynchronous operation</returns>
        Task ClearAllAsync();

        /// <summary>
        /// Gets the total number of patterns in storage
        /// </summary>
        /// <returns>A task containing the count of patterns</returns>
        Task<long> GetPatternCountAsync();
    }
} 