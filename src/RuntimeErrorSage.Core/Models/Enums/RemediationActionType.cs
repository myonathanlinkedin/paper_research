namespace RuntimeErrorSage.Core.Models.Enums
{
    /// <summary>
    /// Represents the type of remediation action.
    /// </summary>
    public enum RemediationActionType
    {
        /// <summary>
        /// Action that monitors system components.
        /// </summary>
        Monitor = 0,

        /// <summary>
        /// Action that sends alerts about system issues.
        /// </summary>
        Alert = 1,

        /// <summary>
        /// Action that creates backups of system components.
        /// </summary>
        Backup = 2,

        /// <summary>
        /// Action that restores system components from backups.
        /// </summary>
        Restore = 3,

        /// <summary>
        /// Action that scales system components.
        /// </summary>
        Scale = 4,

        /// <summary>
        /// Action that updates system components.
        /// </summary>
        Update = 5,

        /// <summary>
        /// Action that rolls back system components.
        /// </summary>
        Rollback = 6,

        /// <summary>
        /// Action that repairs system components.
        /// </summary>
        Repair = 7,

        /// <summary>
        /// Action that reconfigures system components.
        /// </summary>
        Reconfigure = 8,

        /// <summary>
        /// Action that restarts system components.
        /// </summary>
        Restart = 9,

        /// <summary>
        /// Action that performs custom remediation.
        /// </summary>
        Custom = 10
    }
} 