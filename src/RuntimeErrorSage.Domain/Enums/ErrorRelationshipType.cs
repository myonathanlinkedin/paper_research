namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Represents the type of relationship between errors.
    /// </summary>
    public enum ErrorRelationshipType
    {
        /// <summary>
        /// No relationship exists.
        /// </summary>
        None = -1,

        /// <summary>
        /// The relationship type is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// One error causes another.
        /// </summary>
        Causes = 1,

        /// <summary>
        /// One error is caused by another.
        /// </summary>
        CausedBy = 2,

        /// <summary>
        /// One error is related to another.
        /// </summary>
        RelatedTo = 3,

        /// <summary>
        /// One error is similar to another.
        /// </summary>
        SimilarTo = 4,

        /// <summary>
        /// One error depends on another.
        /// </summary>
        DependsOn = 5,

        /// <summary>
        /// One error is a parent of another.
        /// </summary>
        ParentOf = 6,

        /// <summary>
        /// One error is a child of another.
        /// </summary>
        ChildOf = 7,

        /// <summary>
        /// Errors are siblings (share the same parent).
        /// </summary>
        Sibling = 8,

        /// <summary>
        /// One error has a dependency relationship with another.
        /// </summary>
        Dependency = 9,

        /// <summary>
        /// Errors are correlated (occur together).
        /// </summary>
        Correlation = 10,

        /// <summary>
        /// Errors are related in time.
        /// </summary>
        Temporal = 11,

        /// <summary>
        /// Errors are related in location.
        /// </summary>
        Spatial = 12,

        /// <summary>
        /// Errors are logically related.
        /// </summary>
        Logical = 13
    }
} 
