using System.Threading.Tasks;
using System.Collections.Generic;
using RuntimeErrorSage.Model.Models.Backup;

namespace RuntimeErrorSage.Model.Interfaces
{
    /// <summary>
    /// Interface for backup service operations.
    /// </summary>
    public interface IBackupService
    {
        /// <summary>
        /// Creates a backup for the specified application.
        /// </summary>
        /// <param name="applicationId">The application identifier.</param>
        /// <returns>The backup result.</returns>
        Task<BackupResult> CreateBackupAsync(string applicationId);

        /// <summary>
        /// Restores a backup for the specified application.
        /// </summary>
        /// <param name="applicationId">The application identifier.</param>
        /// <param name="backupId">The backup identifier.</param>
        /// <returns>The restore result.</returns>
        Task<RestoreResult> RestoreBackupAsync(string applicationId, string backupId);

        /// <summary>
        /// Lists available backups for the specified application.
        /// </summary>
        /// <param name="applicationId">The application identifier.</param>
        /// <returns>A list of available backups.</returns>
        Task<BackupListResult> ListBackupsAsync(string applicationId);
    }
} 