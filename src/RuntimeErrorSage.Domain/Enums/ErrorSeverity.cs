namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the severity levels for errors.
    /// </summary>
    public enum ErrorSeverity
    {
        /// <summary>
        /// No severity - used for non-error conditions.
        /// </summary>
        None = -1,

        /// <summary>
        /// Fatal severity - system has crashed or cannot recover.
        /// </summary>
        Fatal = -2,

        /// <summary>
        /// Critical severity - system is unusable or data is at risk.
        /// </summary>
        Critical = 0,

        /// <summary>
        /// Error severity - standard error level.
        /// </summary>
        Error = 1,

        /// <summary>
        /// High severity - major functionality is affected.
        /// </summary>
        High = 2,

        /// <summary>
        /// Medium severity - functionality is impaired but not blocked.
        /// </summary>
        Medium = 3,

        /// <summary>
        /// Warning severity - potential issues detected.
        /// </summary>
        Warning = 4,

        /// <summary>
        /// Low severity - minor issues or warnings.
        /// </summary>
        Low = 5,

        /// <summary>
        /// Informational severity - no impact on functionality.
        /// </summary>
        Info = 6,
        
        /// <summary>
        /// Success severity - operation completed successfully.
        /// </summary>
        Success = 7,
        
        /// <summary>
        /// Unknown severity - severity level cannot be determined.
        /// </summary>
        Unknown = 8
    }
} 
