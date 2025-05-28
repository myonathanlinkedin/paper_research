using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.Interfaces
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

// BackupResult.cs
namespace RuntimeErrorSage.Core.Models.Backup
{
    /// <summary>
    /// Represents the result of a backup operation.
    /// </summary>
    public class BackupResult
    {
        /// <summary>
        /// Gets or sets whether the backup was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the backup identifier.
        /// </summary>
        public string BackupId { get; set; }

        /// <summary>
        /// Gets or sets the error message if the backup failed.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}

// RestoreResult.cs
namespace RuntimeErrorSage.Core.Models.Backup
{
    /// <summary>
    /// Represents the result of a restore operation.
    /// </summary>
    public class RestoreResult
    {
        /// <summary>
        /// Gets or sets whether the restore was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error message if the restore failed.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}

// BackupListResult.cs
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Backup
{
    /// <summary>
    /// Represents the result of listing backups.
    /// </summary>
    public class BackupListResult
    {
        /// <summary>
        /// Gets or sets whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the list of backup identifiers.
        /// </summary>
        public List<string> BackupIds { get; set; } = new();

        /// <summary>
        /// Gets or sets the error message if the operation failed.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
} 

