using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Models.Backup
{
    /// <summary>
    /// Represents the result of a restore operation.
    /// </summary>
    public class RestoreResult
    {
        /// <summary>
        /// Gets or sets whether the restore was successful.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets or sets the error message if the restore failed.
        /// </summary>
        public string ErrorMessage { get; }
    }
} 





