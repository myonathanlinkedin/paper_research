namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the values of validation operations.
    /// </summary>
    public enum ValidationValue
    {
        /// <summary>
        /// Unknown validation value.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Valid validation value.
        /// </summary>
        Valid = 1,

        /// <summary>
        /// Invalid validation value.
        /// </summary>
        Invalid = 2,

        /// <summary>
        /// Warning validation value.
        /// </summary>
        Warning = 3,

        /// <summary>
        /// Error validation value.
        /// </summary>
        Error = 4,

        /// <summary>
        /// Critical validation value.
        /// </summary>
        Critical = 5,

        /// <summary>
        /// Fatal validation value.
        /// </summary>
        Fatal = 6
    }
} 
