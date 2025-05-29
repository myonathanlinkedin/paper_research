using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents metrics for a resource.
    /// </summary>
    public class ResourceMetrics
    {
        /// <summary>
        /// Gets or sets the type of resource.
        /// </summary>
        public string ResourceType { get; }

        /// <summary>
        /// Gets or sets the unit of measurement.
        /// </summary>
        public string Unit { get; }

        /// <summary>
        /// Gets or sets the current usage.
        /// </summary>
        public double CurrentUsage { get; }

        /// <summary>
        /// Gets or sets the peak usage.
        /// </summary>
        public double PeakUsage { get; }

        /// <summary>
        /// Gets or sets the usage history.
        /// </summary>
        public IReadOnlyCollection<UsageHistory> UsageHistory { get; }
    }
} 

