namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the sources of validation operations.
    /// </summary>
    public enum ValidationSource
    {
        /// <summary>
        /// Unknown validation source.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// System validation source.
        /// </summary>
        System = 1,

        /// <summary>
        /// Application validation source.
        /// </summary>
        Application = 2,

        /// <summary>
        /// Service validation source.
        /// </summary>
        Service = 3,

        /// <summary>
        /// Component validation source.
        /// </summary>
        Component = 4,

        /// <summary>
        /// Module validation source.
        /// </summary>
        Module = 5,

        /// <summary>
        /// User validation source.
        /// </summary>
        User = 6
    }
} 
