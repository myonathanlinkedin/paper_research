namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the severity levels of validation operations.
    /// </summary>
    public enum ValidationSeverity
    {
        /// <summary>
        /// Unknown validation severity.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Informational validation severity.
        /// </summary>
        Info = 1,

        /// <summary>
        /// Warning validation severity.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Error validation severity.
        /// </summary>
        Error = 3,

        /// <summary>
        /// Critical validation severity.
        /// </summary>
        Critical = 4,

        /// <summary>
        /// Fatal validation severity.
        /// </summary>
        Fatal = 5
    }
} 

