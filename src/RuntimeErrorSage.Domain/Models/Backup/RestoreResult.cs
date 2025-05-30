using System;

namespace RuntimeErrorSage.Domain.Models.Backup
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
