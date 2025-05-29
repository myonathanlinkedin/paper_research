namespace RuntimeErrorSage.Model.Models.Enums;

/// <summary>
/// Defines the type of an error pattern.
/// </summary>
public enum ErrorPatternEnum
{
    /// <summary>
    /// The pattern type is unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The pattern is a syntax error.
    /// </summary>
    SyntaxError = 1,

    /// <summary>
    /// The pattern is a runtime error.
    /// </summary>
    RuntimeError = 2,

    /// <summary>
    /// The pattern is a logical error.
    /// </summary>
    LogicalError = 3,

    /// <summary>
    /// The pattern is a resource error.
    /// </summary>
    ResourceError = 4,

    /// <summary>
    /// The pattern is a configuration error.
    /// </summary>
    ConfigurationError = 5,

    /// <summary>
    /// The pattern is a network error.
    /// </summary>
    NetworkError = 6,

    /// <summary>
    /// The pattern is a security error.
    /// </summary>
    SecurityError = 7,

    /// <summary>
    /// The pattern is a performance error.
    /// </summary>
    PerformanceError = 8,

    /// <summary>
    /// The pattern is a concurrency error.
    /// </summary>
    ConcurrencyError = 9,

    /// <summary>
    /// The pattern is a data error.
    /// </summary>
    DataError = 10,

    /// <summary>
    /// The pattern is a validation error.
    /// </summary>
    ValidationError = 11,

    /// <summary>
    /// The pattern is a business logic error.
    /// </summary>
    BusinessLogicError = 12,

    /// <summary>
    /// The pattern is a system error.
    /// </summary>
    SystemError = 13,

    /// <summary>
    /// The pattern is an application error.
    /// </summary>
    ApplicationError = 14,

    /// <summary>
    /// The pattern is a framework error.
    /// </summary>
    FrameworkError = 15,

    /// <summary>
    /// The pattern is a library error.
    /// </summary>
    LibraryError = 16,

    /// <summary>
    /// The pattern is a third-party error.
    /// </summary>
    ThirdPartyError = 17,

    /// <summary>
    /// The pattern is a custom error.
    /// </summary>
    CustomError = 18
} 
