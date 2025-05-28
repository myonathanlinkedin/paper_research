namespace RuntimeErrorSage.Core.Models.Enums
{
    /// <summary>
    /// Represents the severity level of an error or event.
    /// </summary>
    public enum SeverityLevel
    {
        /// <summary>
        /// Low severity level. Informational events that don't impact functionality.
        /// </summary>
        Low = 0,

        /// <summary>
        /// Medium severity level. Events that might affect functionality but don't cause system failure.
        /// </summary>
        Medium = 1,

        /// <summary>
        /// High severity level. Serious issues that impact core functionality.
        /// </summary>
        High = 2,

        /// <summary>
        /// Critical severity level. System-breaking issues that require immediate attention.
        /// </summary>
        Critical = 3
    }
} 