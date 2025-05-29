using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Application.Models.Enums;

/// <summary>
/// Defines the category of an error.
/// </summary>
public enum ErrorCategory
{
    /// <summary>
    /// The category is unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The error is a syntax error.
    /// </summary>
    Syntax = 1,

    /// <summary>
    /// The error is a runtime error.
    /// </summary>
    Runtime = 2,

    /// <summary>
    /// The error is a logical error.
    /// </summary>
    Logical = 3,

    /// <summary>
    /// The error is a resource error.
    /// </summary>
    Resource = 4,

    /// <summary>
    /// The error is a configuration error.
    /// </summary>
    Configuration = 5,

    /// <summary>
    /// The error is a network error.
    /// </summary>
    Network = 6,

    /// <summary>
    /// The error is a security error.
    /// </summary>
    Security = 7,

    /// <summary>
    /// The error is a performance error.
    /// </summary>
    Performance = 8,

    /// <summary>
    /// The error is a concurrency error.
    /// </summary>
    Concurrency = 9,

    /// <summary>
    /// The error is a data error.
    /// </summary>
    Data = 10,

    /// <summary>
    /// The error is a validation error.
    /// </summary>
    Validation = 11,

    /// <summary>
    /// The error is a business logic error.
    /// </summary>
    BusinessLogic = 12,

    /// <summary>
    /// The error is a system error.
    /// </summary>
    System = 13,

    /// <summary>
    /// The error is an application error.
    /// </summary>
    Application = 14,

    /// <summary>
    /// The error is a framework error.
    /// </summary>
    Framework = 15,

    /// <summary>
    /// The error is a library error.
    /// </summary>
    Library = 16,

    /// <summary>
    /// The error is a third-party error.
    /// </summary>
    ThirdParty = 17,

    /// <summary>
    /// The error is a custom error.
    /// </summary>
    Custom = 18
} 






