namespace RuntimeErrorSage.Core.Models.Validation
{
    /// <summary>
    /// Represents the severity level of a validation result.
    /// </summary>
    public enum SeverityLevel
    {
        /// <summary>
        /// Informational severity - no impact on functionality.
        /// </summary>
        Info = 0,

        /// <summary>
        /// Low severity - minor issues that don't affect functionality.
        /// </summary>
        Low = 1,

        /// <summary>
        /// Medium severity - issues that may affect functionality but have workarounds.
        /// </summary>
        Medium = 2,

        /// <summary>
        /// High severity - critical issues that affect functionality.
        /// </summary>
        High = 3,

        /// <summary>
        /// Critical severity - issues that cause system failure.
        /// </summary>
        Critical = 4
    }
} 