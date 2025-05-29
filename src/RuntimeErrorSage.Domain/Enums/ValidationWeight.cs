namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the weights of validation operations.
    /// </summary>
    public enum ValidationWeight
    {
        /// <summary>
        /// Unknown validation weight.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Light validation weight.
        /// </summary>
        Light = 1,

        /// <summary>
        /// Medium validation weight.
        /// </summary>
        Medium = 2,

        /// <summary>
        /// Heavy validation weight.
        /// </summary>
        Heavy = 3,

        /// <summary>
        /// Critical validation weight.
        /// </summary>
        Critical = 4,

        /// <summary>
        /// Fatal validation weight.
        /// </summary>
        Fatal = 5
    }
} 