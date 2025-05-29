namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Represents the type of relationship between errors.
    /// </summary>
    public enum ErrorRelationshipType
    {
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
        ChildOf = 7
    }
} 
