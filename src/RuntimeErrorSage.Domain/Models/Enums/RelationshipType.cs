namespace RuntimeErrorSage.Model.Models.Enums;

/// <summary>
/// Represents the type of relationship between components in a system.
/// </summary>
public enum RelationshipType
{
    /// <summary>
    /// Components have no direct relationship.
    /// </summary>
    None = 0,

    /// <summary>
    /// Components have a direct dependency relationship.
    /// </summary>
    DirectDependency = 1,

    /// <summary>
    /// Components are related through service calls.
    /// </summary>
    ServiceCall = 2,

    /// <summary>
    /// Components are related through data flow.
    /// </summary>
    DataFlow = 3,

    /// <summary>
    /// Components have an indirect relationship.
    /// </summary>
    Indirect = 4
} 
