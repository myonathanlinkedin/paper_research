namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Represents the scope of impact for a remediation action.
/// </summary>
public enum ImpactScope
{
    /// <summary>
    /// Impact is limited to a single component.
    /// </summary>
    Component,

    /// <summary>
    /// Impact affects multiple components.
    /// </summary>
    MultiComponent,

    /// <summary>
    /// Impact affects the entire system.
    /// </summary>
    System,

    /// <summary>
    /// Impact affects external systems.
    /// </summary>
    External,
    
    /// <summary>
    /// Impact is limited to a local context.
    /// </summary>
    Local,
    
    /// <summary>
    /// Impact affects a service.
    /// </summary>
    Service
} 
