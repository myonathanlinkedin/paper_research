namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the severity levels for errors.
    /// </summary>
    public enum ErrorSeverity
    {
        /// <summary>
        /// Critical severity - system is unusable or data is at risk.
        /// </summary>
        Critical = 0,

        /// <summary>
        /// High severity - major functionality is affected.
        /// </summary>
        High = 1,

        /// <summary>
        /// Medium severity - functionality is impaired but not blocked.
        /// </summary>
        Medium = 2,

        /// <summary>
        /// Low severity - minor issues or warnings.
        /// </summary>
        Low = 3,

        /// <summary>
        /// Informational severity - no impact on functionality.
        /// </summary>
        Info = 4
    }
} 
