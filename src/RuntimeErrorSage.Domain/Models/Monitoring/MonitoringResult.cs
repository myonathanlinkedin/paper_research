using System;

namespace RuntimeErrorSage.Application.Models.Monitoring
{
    /// <summary>
    /// Represents the result of a monitoring operation.
    /// </summary>
    public class MonitoringResult
    {
        /// <summary>
        /// Gets or sets whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Gets or sets the error message if the operation failed.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
} 