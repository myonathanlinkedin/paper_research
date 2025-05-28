using System;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Defines the availability status of a remediation action.
    /// </summary>
    public enum AvailabilityStatus
    {
        /// <summary>
        /// The action is enabled and available.
        /// </summary>
        Enabled,

        /// <summary>
        /// The action is disabled and unavailable.
        /// </summary>
        Disabled,

        /// <summary>
        /// The action is in maintenance mode.
        /// </summary>
        Maintenance,

        /// <summary>
        /// The action's availability status is unknown.
        /// </summary>
        Unknown
    }
} 