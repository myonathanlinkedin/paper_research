namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the types of validation that can be performed.
    /// </summary>
    public enum ValidationType
    {
        /// <summary>
        /// No validation type specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Data validation.
        /// </summary>
        Data = 1,

        /// <summary>
        /// Business rule validation.
        /// </summary>
        BusinessRule = 2,

        /// <summary>
        /// Schema validation.
        /// </summary>
        Schema = 3,

        /// <summary>
        /// Format validation.
        /// </summary>
        Format = 4,

        /// <summary>
        /// Type validation.
        /// </summary>
        Type = 5,

        /// <summary>
        /// Range validation.
        /// </summary>
        Range = 6,

        /// <summary>
        /// Pattern validation.
        /// </summary>
        Pattern = 7,

        /// <summary>
        /// Dependency validation.
        /// </summary>
        Dependency = 8,

        /// <summary>
        /// Security validation.
        /// </summary>
        Security = 9,

        /// <summary>
        /// Performance validation.
        /// </summary>
        Performance = 10,

        /// <summary>
        /// Custom validation.
        /// </summary>
        Custom = 11
    }
} 
