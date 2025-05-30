namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the scope of impact for a remediation action.
/// </summary>
public enum ImpactScope
{
    /// <summary>
    /// No impact scope specified.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Unknown impact scope.
    /// </summary>
    Unknown = 1,
    
    /// <summary>
    /// Impact is limited to a single component.
    /// </summary>
    Component = 2,

    /// <summary>
    /// Impact affects multiple components.
    /// </summary>
    MultiComponent = 3,

    /// <summary>
    /// Impact affects the entire system.
    /// </summary>
    System = 4,

    /// <summary>
    /// Impact affects external systems.
    /// </summary>
    External = 5,
    
    /// <summary>
    /// Impact is limited to a local context.
    /// </summary>
    Local = 6,
    
    /// <summary>
    /// Impact affects a service.
    /// </summary>
    Service = 7
} 
