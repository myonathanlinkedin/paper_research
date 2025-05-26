namespace RuntimeErrorSage.Core.Models.Remediation;

/// <summary>
/// Defines impact scopes for remediation actions.
/// </summary>
public enum RemediationActionImpactScope
{
    /// <summary>
    /// Impact is isolated to a single component.
    /// </summary>
    Component = 0,

    /// <summary>
    /// Impact affects a module or subsystem.
    /// </summary>
    Module = 1,

    /// <summary>
    /// Impact affects a service.
    /// </summary>
    Service = 2,

    /// <summary>
    /// Impact affects multiple services.
    /// </summary>
    MultiService = 3,

    /// <summary>
    /// Impact affects the entire system.
    /// </summary>
    System = 4,

    /// <summary>
    /// Impact affects external dependencies.
    /// </summary>
    External = 5,

    /// <summary>
    /// Impact affects data or storage.
    /// </summary>
    Data = 6,

    /// <summary>
    /// Impact affects network or connectivity.
    /// </summary>
    Network = 7,

    /// <summary>
    /// Impact affects security or access control.
    /// </summary>
    Security = 8,

    /// <summary>
    /// Impact affects performance settings.
    /// </summary>
    Performance,

    /// <summary>
    /// Impact is global across the entire application.
    /// </summary>
    Global,

    /// <summary>
    /// Impact scope is unknown.
    /// </summary>
    Unknown
} 