using System.Collections.ObjectModel;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Storage;

namespace RuntimeErrorSage.Application.Interfaces
{
    /// <summary>
    /// Interface for distributed storage operations.
    /// </summary>
    public interface IDistributedStorage
    {
        /// <summary>
        /// Gets whether the storage is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the storage name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the storage version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets whether the storage is connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Connects to the storage.
        /// </summary>
        Task ConnectAsync();

        /// <summary>
        /// Disconnects from the storage.
        /// </summary>
        Task DisconnectAsync();

        /// <summary>
        /// Stores an error pattern.
        /// </summary>
        /// <param name="pattern">The error pattern.</param>
        Task StorePatternAsync(ErrorPattern pattern);

        /// <summary>
        /// Loads error patterns.
        /// </summary>
        /// <returns>The list of error patterns.</returns>
        Task<Collection<ErrorPattern>> LoadPatternsAsync();

        /// <summary>
        /// Gets an error pattern by ID.
        /// </summary>
        /// <param name="patternId">The pattern ID.</param>
        /// <returns>The error pattern if found, null otherwise.</returns>
        Task<ErrorPattern> GetPatternAsync(string patternId);

        /// <summary>
        /// Updates an error pattern.
        /// </summary>
        /// <param name="pattern">The error pattern.</param>
        Task UpdatePatternAsync(ErrorPattern pattern);

        /// <summary>
        /// Deletes an error pattern.
        /// </summary>
        /// <param name="patternId">The pattern ID.</param>
        Task DeletePatternAsync(string patternId);

        /// <summary>
        /// Gets the storage metrics.
        /// </summary>
        /// <returns>The storage metrics.</returns>
        Task<StorageMetrics> GetMetricsAsync();
    }
} 







