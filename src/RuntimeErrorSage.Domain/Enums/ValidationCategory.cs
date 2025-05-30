namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the categories of validation that can be performed.
    /// </summary>
    public enum ValidationCategory
    {
        /// <summary>
        /// No validation category specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Input validation.
        /// </summary>
        Input = 1,

        /// <summary>
        /// Business rule validation.
        /// </summary>
        BusinessRule = 2,

        /// <summary>
        /// Data integrity validation.
        /// </summary>
        DataIntegrity = 3,

        /// <summary>
        /// Security validation.
        /// </summary>
        Security = 4,

        /// <summary>
        /// Performance validation.
        /// </summary>
        Performance = 5,

        /// <summary>
        /// Configuration validation.
        /// </summary>
        Configuration = 6,

        /// <summary>
        /// Dependency validation.
        /// </summary>
        Dependency = 7,

        /// <summary>
        /// State validation.
        /// </summary>
        State = 8,

        /// <summary>
        /// Format validation.
        /// </summary>
        Format = 9,

        /// <summary>
        /// Compatibility validation.
        /// </summary>
        Compatibility = 10,

        /// <summary>
        /// Resource validation.
        /// </summary>
        Resource = 11,

        /// <summary>
        /// Custom validation.
        /// </summary>
        Custom = 12
    }
} 
