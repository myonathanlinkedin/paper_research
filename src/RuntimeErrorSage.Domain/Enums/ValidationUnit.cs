namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the units of validation operations.
    /// </summary>
    public enum ValidationUnit
    {
        /// <summary>
        /// Unknown validation unit.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Method validation unit.
        /// </summary>
        Method = 1,

        /// <summary>
        /// Property validation unit.
        /// </summary>
        Property = 2,

        /// <summary>
        /// Parameter validation unit.
        /// </summary>
        Parameter = 3,

        /// <summary>
        /// Field validation unit.
        /// </summary>
        Field = 4,

        /// <summary>
        /// Class validation unit.
        /// </summary>
        Class = 5,

        /// <summary>
        /// Interface validation unit.
        /// </summary>
        Interface = 6,

        /// <summary>
        /// Struct validation unit.
        /// </summary>
        Struct = 7,

        /// <summary>
        /// Enum validation unit.
        /// </summary>
        Enum = 8,

        /// <summary>
        /// Delegate validation unit.
        /// </summary>
        Delegate = 9,

        /// <summary>
        /// Event validation unit.
        /// </summary>
        Event = 10
    }
} 