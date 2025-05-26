namespace RuntimeErrorSage.Core.Models.Common
{
    /// <summary>
    /// Represents the severity level of an issue, error, or warning.
    /// </summary>
    public enum SeverityLevel
    {
        /// <summary>
        /// Informational level, no action required.
        /// </summary>
        Info,

        /// <summary>
        /// Low severity, action recommended but not required.
        /// </summary>
        Low,

        /// <summary>
        /// Medium severity, action recommended.
        /// </summary>
        Medium,

        /// <summary>
        /// High severity, action required.
        /// </summary>
        High,

        /// <summary>
        /// Critical severity, immediate action required.
        /// </summary>
        Critical
    }
} 
