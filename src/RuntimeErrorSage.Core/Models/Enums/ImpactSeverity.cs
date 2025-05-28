namespace RuntimeErrorSage.Core.Models.Enums
{
    /// <summary>
    /// Defines the severity levels for impact assessment.
    /// </summary>
    public enum ImpactSeverity
    {
        /// <summary>
        /// Critical impact - system is unusable or data is at risk.
        /// </summary>
        Critical = 0,

        /// <summary>
        /// High impact - major functionality is affected.
        /// </summary>
        High = 1,

        /// <summary>
        /// Medium impact - functionality is impaired but not blocked.
        /// </summary>
        Medium = 2,

        /// <summary>
        /// Low impact - minor issues with minimal effect.
        /// </summary>
        Low = 3,

        /// <summary>
        /// Minimal impact - negligible effect on functionality.
        /// </summary>
        Minimal = 4
    }
} 