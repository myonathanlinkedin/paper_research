namespace RuntimeErrorSage.Core.Storage.Exceptions;

/// <summary>
/// Exception thrown when pattern storage operations fail
/// </summary>
public class PatternStorageException : Exception
{
    /// <summary>
    /// Initializes a new instance of the PatternStorageException class
    /// </summary>
    public PatternStorageException() : base() { }

    /// <summary>
    /// Initializes a new instance of the PatternStorageException class with a message
    /// </summary>
    /// <param name="message">The error message</param>
    public PatternStorageException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the PatternStorageException class with a message and inner exception
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    public PatternStorageException(string message, Exception innerException) 
        : base(message, innerException) { }
} 