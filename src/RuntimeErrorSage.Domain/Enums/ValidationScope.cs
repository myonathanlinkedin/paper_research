namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the scope of validation operations.
    /// </summary>
    public enum ValidationScope
    {
        /// <summary>
        /// Unknown validation scope.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Local validation scope.
        /// </summary>
        Local = 1,

        /// <summary>
        /// Global validation scope.
        /// </summary>
        Global = 2,

        /// <summary>
        /// Component validation scope.
        /// </summary>
        Component = 3,

        /// <summary>
        /// System validation scope.
        /// </summary>
        System = 4,

        /// <summary>
        /// Environment validation scope.
        /// </summary>
        Environment = 5
    }
} 
