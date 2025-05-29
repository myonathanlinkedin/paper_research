namespace RuntimeErrorSage.Application.Models.Enums
{
    /// <summary>
    /// Defines the categories of validation operations.
    /// </summary>
    public enum ValidationCategory
    {
        /// <summary>
        /// Unknown category.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Input validation.
        /// </summary>
        Input = 1,

        /// <summary>
        /// Output validation.
        /// </summary>
        Output = 2,

        /// <summary>
        /// State validation.
        /// </summary>
        State = 3,

        /// <summary>
        /// Configuration validation.
        /// </summary>
        Configuration = 4,

        /// <summary>
        /// Security validation.
        /// </summary>
        Security = 5,

        /// <summary>
        /// Business rule validation.
        /// </summary>
        BusinessRule = 6,

        /// <summary>
        /// Performance validation.
        /// </summary>
        Performance = 7,

        /// <summary>
        /// Compatibility validation.
        /// </summary>
        Compatibility = 8,

        /// <summary>
        /// Health validation.
        /// </summary>
        Health = 9,

        /// <summary>
        /// Dependency validation.
        /// </summary>
        Dependency = 10
    }
} 
