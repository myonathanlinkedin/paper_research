using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Model.Models.Remediation
{
    /// <summary>
    /// Represents metrics for a resource.
    /// </summary>
    public class ResourceMetrics
    {
        /// <summary>
        /// Gets or sets the type of resource.
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the unit of measurement.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the current usage.
        /// </summary>
        public double CurrentUsage { get; set; }

        /// <summary>
        /// Gets or sets the peak usage.
        /// </summary>
        public double PeakUsage { get; set; }

        /// <summary>
        /// Gets or sets the usage history.
        /// </summary>
        public List<UsageRecord> UsageHistory { get; set; }
    }
} 