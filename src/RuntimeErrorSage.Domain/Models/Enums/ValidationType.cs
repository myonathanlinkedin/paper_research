namespace RuntimeErrorSage.Application.Models.Enums
{
    /// <summary>
    /// Defines the types of validation operations.
    /// </summary>
    public enum ValidationType
    {
        /// <summary>
        /// Unknown validation type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Schema validation.
        /// </summary>
        Schema = 1,

        /// <summary>
        /// Business rule validation.
        /// </summary>
        BusinessRule = 2,

        /// <summary>
        /// Format validation.
        /// </summary>
        Format = 3,

        /// <summary>
        /// Consistency validation.
        /// </summary>
        Consistency = 4,

        /// <summary>
        /// Security validation.
        /// </summary>
        Security = 5,

        /// <summary>
        /// Performance validation.
        /// </summary>
        Performance = 6,

        /// <summary>
        /// Compatibility validation.
        /// </summary>
        Compatibility = 7,

        /// <summary>
        /// Dependency validation.
        /// </summary>
        Dependency = 8,

        /// <summary>
        /// Configuration validation.
        /// </summary>
        Configuration = 9,

        /// <summary>
        /// Health validation.
        /// </summary>
        Health = 10
    }
} 
