namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the scope of validation operations.
    /// </summary>
    public enum ValidationScope
    {
        /// <summary>
        /// No scope specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Validation applies to a single property.
        /// </summary>
        Property = 1,

        /// <summary>
        /// Validation applies to a single object.
        /// </summary>
        Object = 2,

        /// <summary>
        /// Validation applies to a collection of objects.
        /// </summary>
        Collection = 3,

        /// <summary>
        /// Validation applies to a component.
        /// </summary>
        Component = 4,

        /// <summary>
        /// Validation applies to a module.
        /// </summary>
        Module = 5,

        /// <summary>
        /// Validation applies to a service.
        /// </summary>
        Service = 6,

        /// <summary>
        /// Validation applies to the entire system.
        /// </summary>
        System = 7,

        /// <summary>
        /// Validation applies to external dependencies.
        /// </summary>
        External = 8,

        /// <summary>
        /// Validation applies to the global scope.
        /// </summary>
        Global = 9
    }
} 
