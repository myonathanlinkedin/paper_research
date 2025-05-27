namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the source of an error.
/// </summary>
public enum ErrorSource
{
    /// <summary>
    /// The source is unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The error is from the application.
    /// </summary>
    Application = 1,

    /// <summary>
    /// The error is from the framework.
    /// </summary>
    Framework = 2,

    /// <summary>
    /// The error is from a library.
    /// </summary>
    Library = 3,

    /// <summary>
    /// The error is from a third-party component.
    /// </summary>
    ThirdParty = 4,

    /// <summary>
    /// The error is from the system.
    /// </summary>
    System = 5,

    /// <summary>
    /// The error is from the network.
    /// </summary>
    Network = 6,

    /// <summary>
    /// The error is from the database.
    /// </summary>
    Database = 7,

    /// <summary>
    /// The error is from the file system.
    /// </summary>
    FileSystem = 8,

    /// <summary>
    /// The error is from the user interface.
    /// </summary>
    UserInterface = 9,

    /// <summary>
    /// The error is from the business logic.
    /// </summary>
    BusinessLogic = 10,

    /// <summary>
    /// The error is from the configuration.
    /// </summary>
    Configuration = 11,

    /// <summary>
    /// The error is from the security system.
    /// </summary>
    Security = 12,

    /// <summary>
    /// The error is from the performance monitoring.
    /// </summary>
    Performance = 13,

    /// <summary>
    /// The error is from the concurrency system.
    /// </summary>
    Concurrency = 14,

    /// <summary>
    /// The error is from the data validation.
    /// </summary>
    DataValidation = 15,

    /// <summary>
    /// The error is from the custom code.
    /// </summary>
    Custom = 16
} 