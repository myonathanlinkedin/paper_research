using System;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Defines the types of events that can be audited.
    /// </summary>
    public enum AuditEventType
    {
        /// <summary>
        /// The action was created.
        /// </summary>
        Created,

        /// <summary>
        /// The action was modified.
        /// </summary>
        Modified,

        /// <summary>
        /// The action was executed.
        /// </summary>
        Executed,

        /// <summary>
        /// The action was cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// The action was completed.
        /// </summary>
        Completed,

        /// <summary>
        /// The action failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The action was deleted.
        /// </summary>
        Deleted,

        /// <summary>
        /// The action's status was changed.
        /// </summary>
        StatusChanged,

        /// <summary>
        /// The action's configuration was changed.
        /// </summary>
        ConfigurationChanged,

        /// <summary>
        /// The action's permissions were changed.
        /// </summary>
        PermissionsChanged
    }
} 