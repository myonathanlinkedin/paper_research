using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Backup
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