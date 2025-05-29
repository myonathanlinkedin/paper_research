using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents a record of resource usage at a specific time.
    /// </summary>
    public class UsageRecord
    {
        /// <summary>
        /// Gets or sets the timestamp of the usage record.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets or sets the usage value.
        /// </summary>
        public double Usage { get; }
    }
} 





