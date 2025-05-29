using System;

namespace RuntimeErrorSage.Model.Models.Remediation
{
    /// <summary>
    /// Represents limits for a resource.
    /// </summary>
    public class ResourceLimits
    {
        /// <summary>
        /// Gets or sets the maximum allowed usage.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the warning threshold.
        /// </summary>
        public double WarningThreshold { get; set; }

        /// <summary>
        /// Gets or sets the critical threshold.
        /// </summary>
        public double CriticalThreshold { get; set; }
    }
} 