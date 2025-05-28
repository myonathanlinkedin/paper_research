namespace RuntimeErrorSage.Core.Models.Enums
{
    /// <summary>
    /// Defines the scope of an impact assessment.
    /// </summary>
    public enum ImpactScope
    {
        /// <summary>
        /// Global impact affecting the entire system.
        /// </summary>
        Global = 0,

        /// <summary>
        /// System impact affecting a large portion of the system.
        /// </summary>
        System = 1,

        /// <summary>
        /// Service impact affecting a specific service.
        /// </summary>
        Service = 2,

        /// <summary>
        /// Component impact affecting a specific component.
        /// </summary>
        Component = 3,

        /// <summary>
        /// Local impact affecting a specific area.
        /// </summary>
        Local = 4,

        /// <summary>
        /// User impact affecting specific users.
        /// </summary>
        User = 5
    }
} 