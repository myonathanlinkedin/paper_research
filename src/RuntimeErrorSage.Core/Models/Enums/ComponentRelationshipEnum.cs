namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the type of relationship between components.
/// </summary>
public enum ComponentRelationshipEnum
{
    /// <summary>
    /// No relationship.
    /// </summary>
    None,

    /// <summary>
    /// Direct dependency.
    /// </summary>
    DirectDependency,

    /// <summary>
    /// Indirect dependency.
    /// </summary>
    IndirectDependency,

    /// <summary>
    /// Parent-child relationship.
    /// </summary>
    ParentChild,

    /// <summary>
    /// Sibling relationship.
    /// </summary>
    Sibling,

    /// <summary>
    /// Communication relationship.
    /// </summary>
    Communication,

    /// <summary>
    /// Data flow relationship.
    /// </summary>
    DataFlow,

    /// <summary>
    /// Control flow relationship.
    /// </summary>
    ControlFlow,

    /// <summary>
    /// Shared resource relationship.
    /// </summary>
    SharedResource
} 
