namespace RuntimeErrorSage.Model.Models.Enums
{
    /// <summary>
    /// Represents the severity level of an error, event, or action.
    /// This is the canonical severity enum for the entire system.
    /// </summary>
    public enum SeverityLevel
    {
        /// <summary>
        /// Unknown severity level.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Informational severity level. No impact on functionality.
        /// </summary>
        Info = 1,

        /// <summary>
        /// Low severity level. Minor issues or warnings.
        /// </summary>
        Low = 2,

        /// <summary>
        /// Medium severity level. Functionality is impaired but not blocked.
        /// </summary>
        Medium = 3,

        /// <summary>
        /// High severity level. Major functionality is affected.
        /// </summary>
        High = 4,

        /// <summary>
        /// Critical severity level. System is unusable or data is at risk.
        /// </summary>
        Critical = 5,

        /// <summary>
        /// Catastrophic severity level. System-wide failure or data loss.
        /// </summary>
        Catastrophic = 6
    }
} 
