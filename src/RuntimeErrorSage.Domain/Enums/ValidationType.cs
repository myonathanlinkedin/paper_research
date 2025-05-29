namespace RuntimeErrorSage.Domain.Enums
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
        /// General validation type.
        /// </summary>
        General = 1,

        /// <summary>
        /// Schema validation type.
        /// </summary>
        Schema = 2,

        /// <summary>
        /// Business rule validation type.
        /// </summary>
        BusinessRule = 3,

        /// <summary>
        /// Format validation type.
        /// </summary>
        Format = 4,

        /// <summary>
        /// Consistency validation type.
        /// </summary>
        Consistency = 5,

        /// <summary>
        /// Security validation type.
        /// </summary>
        Security = 6,

        /// <summary>
        /// Performance validation type.
        /// </summary>
        Performance = 7,

        /// <summary>
        /// Compatibility validation type.
        /// </summary>
        Compatibility = 8,

        /// <summary>
        /// Dependency validation type.
        /// </summary>
        Dependency = 9,

        /// <summary>
        /// Configuration validation type.
        /// </summary>
        Configuration = 10,

        /// <summary>
        /// Health validation type.
        /// </summary>
        Health = 11
    }
} 
