using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Defines the interface for distributed storage operations.
    /// </summary>
    public interface IDistributedStorage
    {
        /// <summary>
        /// Stores an error context.
        /// </summary>
        Task StoreContextAsync(ErrorContext context);

        /// <summary>
        /// Retrieves contexts for a service within a time range.
        /// </summary>
        Task<List<ErrorContext>> GetContextsAsync(string serviceName, DateTime startTime, DateTime endTime);

        /// <summary>
        /// Stores an error pattern.
        /// </summary>
        Task StorePatternAsync(ErrorPattern pattern);

        /// <summary>
        /// Retrieves patterns for a service.
        /// </summary>
        Task<List<ErrorPattern>> GetPatternsAsync(string serviceName);

        /// <summary>
        /// Deletes expired data.
        /// </summary>
        Task DeleteExpiredDataAsync();
    }
} 
