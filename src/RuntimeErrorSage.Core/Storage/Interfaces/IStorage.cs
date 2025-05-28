using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Storage.Interfaces
{
    /// <summary>
    /// Represents a storage interface for error contexts and analysis results.
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Gets the storage identifier.
        /// </summary>
        string StorageId { get; }

        /// <summary>
        /// Gets the storage type.
        /// </summary>
        string StorageType { get; }

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
        /// Stores an error context.
        /// </summary>
        /// <param name="errorContext">The error context to store.</param>
        /// <returns>A task representing the store operation.</returns>
        Task StoreErrorContextAsync(ErrorContext errorContext);

        /// <summary>
        /// Retrieves an error context by its identifier.
        /// </summary>
        /// <param name="errorContextId">The error context identifier.</param>
        /// <returns>The error context.</returns>
        Task<ErrorContext?> GetErrorContextAsync(string errorContextId);

        /// <summary>
        /// Stores an error analysis result.
        /// </summary>
        /// <param name="analysisResult">The analysis result to store.</param>
        /// <returns>A task representing the store operation.</returns>
        Task StoreAnalysisResultAsync(ErrorAnalysisResult analysisResult);

        /// <summary>
        /// Retrieves an error analysis result by its identifier.
        /// </summary>
        /// <param name="analysisId">The analysis identifier.</param>
        /// <returns>The analysis result.</returns>
        Task<ErrorAnalysisResult?> GetAnalysisResultAsync(string analysisId);

        /// <summary>
        /// Queries error contexts based on criteria.
        /// </summary>
        /// <param name="criteria">The query criteria.</param>
        /// <returns>The matching error contexts.</returns>
        Task<IEnumerable<ErrorContext>> QueryErrorContextsAsync(Dictionary<string, object> criteria);

        /// <summary>
        /// Queries analysis results based on criteria.
        /// </summary>
        /// <param name="criteria">The query criteria.</param>
        /// <returns>The matching analysis results.</returns>
        Task<IEnumerable<ErrorAnalysisResult>> QueryAnalysisResultsAsync(Dictionary<string, object> criteria);

        /// <summary>
        /// Deletes an error context by its identifier.
        /// </summary>
        /// <param name="errorContextId">The error context identifier.</param>
        /// <returns>A task representing the delete operation.</returns>
        Task DeleteErrorContextAsync(string errorContextId);

        /// <summary>
        /// Deletes an analysis result by its identifier.
        /// </summary>
        /// <param name="analysisId">The analysis identifier.</param>
        /// <returns>A task representing the delete operation.</returns>
        Task DeleteAnalysisResultAsync(string analysisId);

        /// <summary>
        /// Gets the storage statistics.
        /// </summary>
        /// <returns>The storage statistics.</returns>
        Task<Dictionary<string, object>> GetStatisticsAsync();

        /// <summary>
        /// Performs a backup of the storage.
        /// </summary>
        /// <returns>A task representing the backup operation.</returns>
        Task BackupAsync();

        /// <summary>
        /// Restores the storage from a backup.
        /// </summary>
        /// <param name="backupId">The backup identifier.</param>
        /// <returns>A task representing the restore operation.</returns>
        Task RestoreAsync(string backupId);

        /// <summary>
        /// Loads error patterns from storage.
        /// </summary>
        /// <returns>A list of error patterns.</returns>
        Task<List<ErrorPattern>> LoadPatternsAsync();
    }
} 
