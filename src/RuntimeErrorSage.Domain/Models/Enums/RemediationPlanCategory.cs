namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Defines the categories of remediation plans.
/// </summary>
public enum RemediationPlanCategory
{
    /// <summary>
    /// The plan is for infrastructure.
    /// </summary>
    Infrastructure,

    /// <summary>
    /// The plan is for application.
    /// </summary>
    Application,

    /// <summary>
    /// The plan is for database.
    /// </summary>
    Database,

    /// <summary>
    /// The plan is for network.
    /// </summary>
    Network,

    /// <summary>
    /// The plan is for security.
    /// </summary>
    Security,

    /// <summary>
    /// The plan is for performance.
    /// </summary>
    Performance,

    /// <summary>
    /// The plan is for availability.
    /// </summary>
    Availability,

    /// <summary>
    /// The plan is for reliability.
    /// </summary>
    Reliability,

    /// <summary>
    /// The plan is for scalability.
    /// </summary>
    Scalability,

    /// <summary>
    /// The plan is for maintainability.
    /// </summary>
    Maintainability,

    /// <summary>
    /// The plan is for compatibility.
    /// </summary>
    Compatibility,

    /// <summary>
    /// The plan is for usability.
    /// </summary>
    Usability,

    /// <summary>
    /// The plan is for portability.
    /// </summary>
    Portability,

    /// <summary>
    /// The plan is unknown.
    /// </summary>
    Unknown
} 

