using System;
using System.Collections.Generic;

using RuntimeErrorSage.Domain.Models.Remediation;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents the availability of a remediation action.
    /// </summary>
    public class RemediationActionAvailability
    {
        /// <summary>
        /// Gets or sets the list of availability windows.
        /// </summary>
        public List<AvailabilityWindow> Windows { get; set; } = new();

        /// <summary>
        /// Gets or sets the current availability status.
        /// </summary>
        public AvailabilityStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the last status update time.
        /// </summary>
        public DateTime LastStatusUpdate { get; set; }

        /// <summary>
        /// Gets or sets the reason for the current status.
        /// </summary>
        public string StatusReason { get; set; }

        /// <summary>
        /// Gets or sets whether the action is currently available.
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Gets or sets the next available time if currently unavailable.
        /// </summary>
        public DateTime? NextAvailableTime { get; set; }

        /// <summary>
        /// Gets or sets the maintenance window if applicable.
        /// </summary>
        public AvailabilityWindow MaintenanceWindow { get; set; }

        /// <summary>
        /// Gets or sets the list of scheduled maintenance windows.
        /// </summary>
        public List<AvailabilityWindow> ScheduledMaintenance { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of blackout periods.
        /// </summary>
        public List<AvailabilityWindow> BlackoutPeriods { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed time zones.
        /// </summary>
        public List<string> AllowedTimeZones { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed regions.
        /// </summary>
        public List<string> AllowedRegions { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed environments.
        /// </summary>
        public List<string> AllowedEnvironments { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed applications.
        /// </summary>
        public List<string> AllowedApplications { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error types.
        /// </summary>
        public List<string> AllowedErrorTypes { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error severities.
        /// </summary>
        public List<string> AllowedErrorSeverities { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error sources.
        /// </summary>
        public List<string> AllowedErrorSources { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error categories.
        /// </summary>
        public List<string> AllowedErrorCategories { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error subcategories.
        /// </summary>
        public List<string> AllowedErrorSubcategories { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error tags.
        /// </summary>
        public List<string> AllowedErrorTags { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error attributes.
        /// </summary>
        public List<string> AllowedErrorAttributes { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error metadata.
        /// </summary>
        public List<string> AllowedErrorMetadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error context.
        /// </summary>
        public List<string> AllowedErrorContext { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error stack traces.
        /// </summary>
        public List<string> AllowedErrorStackTraces { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error messages.
        /// </summary>
        public List<string> AllowedErrorMessages { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error codes.
        /// </summary>
        public List<string> AllowedErrorCodes { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error exceptions.
        /// </summary>
        public List<string> AllowedErrorExceptions { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error inner exceptions.
        /// </summary>
        public List<string> AllowedErrorInnerExceptions { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error source.
        /// </summary>
        public List<string> AllowedErrorSource { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error target site.
        /// </summary>
        public List<string> AllowedErrorTargetSite { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error help link.
        /// </summary>
        public List<string> AllowedErrorHelpLink { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error data.
        /// </summary>
        public List<string> AllowedErrorData { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error hresult.
        /// </summary>
        public List<string> AllowedErrorHResult { get; set; } = new();
    }
} 
