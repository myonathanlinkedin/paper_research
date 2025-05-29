namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the targets of validation operations.
    /// </summary>
    public enum ValidationTarget
    {
        /// <summary>
        /// Unknown validation target.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Data validation target.
        /// </summary>
        Data = 1,

        /// <summary>
        /// Business validation target.
        /// </summary>
        Business = 2,

        /// <summary>
        /// Security validation target.
        /// </summary>
        Security = 3,

        /// <summary>
        /// Performance validation target.
        /// </summary>
        Performance = 4,

        /// <summary>
        /// Compliance validation target.
        /// </summary>
        Compliance = 5,

        /// <summary>
        /// Configuration validation target.
        /// </summary>
        Configuration = 6
    }
} 