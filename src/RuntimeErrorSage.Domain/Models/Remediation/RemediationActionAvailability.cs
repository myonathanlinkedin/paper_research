using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents the availability of a remediation action.
    /// </summary>
    public class RemediationActionAvailability
    {
        /// <summary>
        /// Gets or sets the list of availability windows.
        /// </summary>
        public IReadOnlyCollection<Windows> Windows { get; } = new();

        /// <summary>
        /// Gets or sets the current availability status.
        /// </summary>
        public AvailabilityStatus Status { get; }

        /// <summary>
        /// Gets or sets the last status update time.
        /// </summary>
        public DateTime LastStatusUpdate { get; }

        /// <summary>
        /// Gets or sets the reason for the current status.
        /// </summary>
        public string StatusReason { get; }

        /// <summary>
        /// Gets or sets whether the action is currently available.
        /// </summary>
        public bool IsAvailable { get; }

        /// <summary>
        /// Gets or sets the next available time if currently unavailable.
        /// </summary>
        public DateTime? NextAvailableTime { get; set; }

        /// <summary>
        /// Gets or sets the maintenance window if applicable.
        /// </summary>
        public AvailabilityWindow MaintenanceWindow { get; }

        /// <summary>
        /// Gets or sets the list of scheduled maintenance windows.
        /// </summary>
        public IReadOnlyCollection<ScheduledMaintenance> ScheduledMaintenance { get; } = new();

        /// <summary>
        /// Gets or sets the list of blackout periods.
        /// </summary>
        public IReadOnlyCollection<BlackoutPeriods> BlackoutPeriods { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed time zones.
        /// </summary>
        public IReadOnlyCollection<AllowedTimeZones> AllowedTimeZones { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed regions.
        /// </summary>
        public IReadOnlyCollection<AllowedRegions> AllowedRegions { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed environments.
        /// </summary>
        public IReadOnlyCollection<AllowedEnvironments> AllowedEnvironments { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed applications.
        /// </summary>
        public IReadOnlyCollection<AllowedApplications> AllowedApplications { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error types.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorTypes> AllowedErrorTypes { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error severities.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorSeverities> AllowedErrorSeverities { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error sources.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorSources> AllowedErrorSources { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error categories.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorCategories> AllowedErrorCategories { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error subcategories.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorSubcategories> AllowedErrorSubcategories { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error tags.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorTags> AllowedErrorTags { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error attributes.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorAttributes> AllowedErrorAttributes { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error metadata.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorMetadata> AllowedErrorMetadata { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error context.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorContext> AllowedErrorContext { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error stack traces.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorStackTraces> AllowedErrorStackTraces { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error messages.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorMessages> AllowedErrorMessages { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error codes.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorCodes> AllowedErrorCodes { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error exceptions.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorExceptions> AllowedErrorExceptions { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error inner exceptions.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorInnerExceptions> AllowedErrorInnerExceptions { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error source.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorSource> AllowedErrorSource { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error target site.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorTargetSite> AllowedErrorTargetSite { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error help link.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorHelpLink> AllowedErrorHelpLink { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error data.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorData> AllowedErrorData { get; } = new();

        /// <summary>
        /// Gets or sets the list of allowed error hresult.
        /// </summary>
        public IReadOnlyCollection<AllowedErrorHResult> AllowedErrorHResult { get; } = new();
    }
} 




