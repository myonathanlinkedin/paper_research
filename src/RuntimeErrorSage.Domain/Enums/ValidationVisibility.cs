namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the visibility levels of validation operations.
    /// </summary>
    public enum ValidationVisibility
    {
        /// <summary>
        /// Unknown validation visibility.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Public validation visibility.
        /// </summary>
        Public = 1,

        /// <summary>
        /// Private validation visibility.
        /// </summary>
        Private = 2,

        /// <summary>
        /// Protected validation visibility.
        /// </summary>
        Protected = 3,

        /// <summary>
        /// Internal validation visibility.
        /// </summary>
        Internal = 4,

        /// <summary>
        /// Protected internal validation visibility.
        /// </summary>
        ProtectedInternal = 5,

        /// <summary>
        /// Private protected validation visibility.
        /// </summary>
        PrivateProtected = 6
    }
} 
