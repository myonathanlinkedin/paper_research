namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the scope of impact for an error or action.
/// </summary>
public enum ImpactScope
{
    /// <summary>
    /// Unknown scope.
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Impact limited to a single component.
    /// </summary>
    SingleComponent = 1,
    
    /// <summary>
    /// Impact limited to multiple components in a service.
    /// </summary>
    MultipleComponents = 2,
    
    /// <summary>
    /// Impact limited to a single service.
    /// </summary>
    SingleService = 3,
    
    /// <summary>
    /// Impact affects multiple services.
    /// </summary>
    MultipleServices = 4,
    
    /// <summary>
    /// Impact affects an entire system.
    /// </summary>
    EntireSystem = 5,
    
    /// <summary>
    /// Impact extends beyond the system to external dependencies.
    /// </summary>
    ExternalDependencies = 6
} 