namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the severity level of an error or issue.
    /// </summary>
    public enum SeverityLevel
    {
        /// <summary>
        /// Unknown severity level.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Informational severity level.
        /// </summary>
        Info = 1,

        /// <summary>
        /// Low severity level.
        /// </summary>
        Low = 2,

        /// <summary>
        /// Medium severity level.
        /// </summary>
        Medium = 3,

        /// <summary>
        /// High severity level.
        /// </summary>
        High = 4,

        /// <summary>
        /// Critical severity level.
        /// </summary>
        Critical = 5,

        /// <summary>
        /// Catastrophic severity level.
        /// </summary>
        Catastrophic = 6
    }
} 
