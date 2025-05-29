namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the scope of a validation operation.
    /// </summary>
    public enum ValidationScope
    {
        /// <summary>
        /// Unknown scope.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// System level scope.
        /// </summary>
        System = 1,

        /// <summary>
        /// Application level scope.
        /// </summary>
        Application = 2,

        /// <summary>
        /// Service level scope.
        /// </summary>
        Service = 3,

        /// <summary>
        /// Module level scope.
        /// </summary>
        Module = 4,

        /// <summary>
        /// Component level scope.
        /// </summary>
        Component = 5,

        /// <summary>
        /// Method level scope.
        /// </summary>
        Method = 6,

        /// <summary>
        /// Property level scope.
        /// </summary>
        Property = 7,

        /// <summary>
        /// Parameter level scope.
        /// </summary>
        Parameter = 8
    }
} 
