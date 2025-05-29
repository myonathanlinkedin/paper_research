namespace RuntimeErrorSage.Domain.Enums;

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
    Component = 1,
    
    /// <summary>
    /// Impact limited to multiple components in a service.
    /// </summary>
    MultipleComponents = 2,
    
    /// <summary>
    /// Impact limited to a single service.
    /// </summary>
    Service = 3,
    
    /// <summary>
    /// Impact affects multiple services.
    /// </summary>
    MultipleServices = 4,
    
    /// <summary>
    /// Impact affects the entire system.
    /// </summary>
    System = 5,
    
    /// <summary>
    /// Impact affects the entire environment.
    /// </summary>
    Environment = 6,
    
    /// <summary>
    /// Impact affects the entire organization.
    /// </summary>
    Organization = 7,
    
    /// <summary>
    /// Impact affects the entire global system.
    /// </summary>
    Global = 8,

    /// <summary>
    /// Impact affects a specific module.
    /// </summary>
    Module = 9,

    /// <summary>
    /// Impact affects a specific local area.
    /// </summary>
    Local = 10
} 
