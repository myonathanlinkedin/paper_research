namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Represents the scope of impact for an error or remediation action.
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
    /// Impact extends beyond the system to external dependencies.
    /// </summary>
    External
} 