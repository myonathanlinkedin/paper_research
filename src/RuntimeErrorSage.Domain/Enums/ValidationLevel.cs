namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the levels of validation to be performed.
    /// </summary>
    public enum ValidationLevel
    {
        /// <summary>
        /// No validation.
        /// </summary>
        None = 0,

        /// <summary>
        /// Basic validation - minimal checks.
        /// </summary>
        Basic = 1,

        /// <summary>
        /// Standard validation - common checks.
        /// </summary>
        Standard = 2,

        /// <summary>
        /// Advanced validation - thorough checks.
        /// </summary>
        Advanced = 3,

        /// <summary>
        /// Strict validation - comprehensive checks.
        /// </summary>
        Strict = 4,

        /// <summary>
        /// Custom validation - user-defined checks.
        /// </summary>
        Custom = 5
    }
} 
