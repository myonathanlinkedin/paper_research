namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the results of validation operations.
    /// </summary>
    public enum ValidationResultEnum
    {
        /// <summary>
        /// Unknown validation result.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Passed validation result.
        /// </summary>
        Passed = 1,

        /// <summary>
        /// Failed validation result.
        /// </summary>
        Failed = 2,

        /// <summary>
        /// Warning validation result.
        /// </summary>
        Warning = 3,

        /// <summary>
        /// Error validation result.
        /// </summary>
        Error = 4,

        /// <summary>
        /// Skipped validation result.
        /// </summary>
        Skipped = 5
    }
} 