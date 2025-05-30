namespace RuntimeErrorSage.Domain.Enums;

/// <summary>
/// Defines the categories of errors that can occur in the system.
/// </summary>
public enum ErrorCategory
{
    /// <summary>
    /// Unknown error category.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Runtime exception error.
    /// </summary>
    RuntimeException = 1,

    /// <summary>
    /// Compilation error.
    /// </summary>
    CompilationError = 2,

    /// <summary>
    /// Configuration error.
    /// </summary>
    Configuration = 3,

    /// <summary>
    /// Database error.
    /// </summary>
    Database = 4,

    /// <summary>
    /// Network error.
    /// </summary>
    Network = 5,

    /// <summary>
    /// Security error.
    /// </summary>
    Security = 6,

    /// <summary>
    /// Performance error.
    /// </summary>
    Performance = 7,

    /// <summary>
    /// Resource error.
    /// </summary>
    Resource = 8,

    /// <summary>
    /// Validation error.
    /// </summary>
    Validation = 9,

    /// <summary>
    /// Business logic error.
    /// </summary>
    BusinessLogic = 10,

    /// <summary>
    /// Integration error.
    /// </summary>
    Integration = 11,

    /// <summary>
    /// Infrastructure error.
    /// </summary>
    Infrastructure = 12,

    /// <summary>
    /// Data error.
    /// </summary>
    Data = 13,

    /// <summary>
    /// System error.
    /// </summary>
    System = 14
} 
