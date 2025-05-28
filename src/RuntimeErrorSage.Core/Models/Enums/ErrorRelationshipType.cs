namespace RuntimeErrorSage.Core.Models.Enums
{
    /// <summary>
    /// Defines the types of relationships between errors.
    /// </summary>
    public enum ErrorRelationshipType
    {
        /// <summary>
        /// Parent-child relationship where one error is caused by another.
        /// </summary>
        ParentChild = 0,

        /// <summary>
        /// Sibling relationship where errors are related but not directly dependent.
        /// </summary>
        Sibling = 1,

        /// <summary>
        /// Correlation relationship where errors tend to occur together.
        /// </summary>
        Correlated = 2,

        /// <summary>
        /// Dependency relationship where one error depends on another.
        /// </summary>
        Dependent = 3,

        /// <summary>
        /// No direct relationship between errors.
        /// </summary>
        None = 4
    }
} 