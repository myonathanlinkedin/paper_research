namespace RuntimeErrorSage.Tests.TestSuite.Enums;

/// <summary>
/// Types of errors that can be tested
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Database-related error
    /// </summary>
    Database,

    /// <summary>
    /// File system error
    /// </summary>
    FileSystem,

    /// <summary>
    /// Network error
    /// </summary>
    Network,

    /// <summary>
    /// Resource allocation error
    /// </summary>
    Resource,

    /// <summary>
    /// Service integration error
    /// </summary>
    Service,

    /// <summary>
    /// Unknown error type
    /// </summary>
    Unknown
} 