using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents limits for a resource.
    /// </summary>
    public class ResourceLimits
    {
        /// <summary>
        /// Gets or sets the maximum allowed usage.
        /// </summary>
        public double Maximum { get; }

        /// <summary>
        /// Gets or sets the warning threshold.
        /// </summary>
        public double WarningThreshold { get; }

        /// <summary>
        /// Gets or sets the critical threshold.
        /// </summary>
        public double CriticalThreshold { get; }
    }
} 




