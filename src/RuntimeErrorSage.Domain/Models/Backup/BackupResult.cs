using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Models.Backup
{
    /// <summary>
    /// Represents the result of a backup operation.
    /// </summary>
    public class BackupResult
    {
        /// <summary>
        /// Gets or sets whether the backup was successful.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets or sets the backup identifier.
        /// </summary>
        public string BackupId { get; }

        /// <summary>
        /// Gets or sets the error message if the backup failed.
        /// </summary>
        public string ErrorMessage { get; }
    }
} 





