namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the categories of validation operations.
    /// </summary>
    public enum ValidationCategory
    {
        /// <summary>
        /// Unknown validation category.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// General validation category.
        /// </summary>
        General = 1,

        /// <summary>
        /// Data validation category.
        /// </summary>
        Data = 2,

        /// <summary>
        /// Business validation category.
        /// </summary>
        Business = 3,

        /// <summary>
        /// Security validation category.
        /// </summary>
        Security = 4,

        /// <summary>
        /// Performance validation category.
        /// </summary>
        Performance = 5,

        /// <summary>
        /// Compliance validation category.
        /// </summary>
        Compliance = 6,

        /// <summary>
        /// Input validation.
        /// </summary>
        Input = 7,

        /// <summary>
        /// Output validation.
        /// </summary>
        Output = 8,

        /// <summary>
        /// State validation.
        /// </summary>
        State = 9,

        /// <summary>
        /// Configuration validation.
        /// </summary>
        Configuration = 10,

        /// <summary>
        /// Business rule validation.
        /// </summary>
        BusinessRule = 11,

        /// <summary>
        /// Compatibility validation.
        /// </summary>
        Compatibility = 12,

        /// <summary>
        /// Health validation.
        /// </summary>
        Health = 13,

        /// <summary>
        /// Dependency validation.
        /// </summary>
        Dependency = 14
    }
} 
