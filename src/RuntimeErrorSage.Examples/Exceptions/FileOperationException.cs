using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Examples.Exceptions;

/// <summary>
/// Exception thrown when a file operation fails
/// </summary>
public class FileOperationException : Exception
{
    /// <summary>
    /// Additional context information about the error
    /// </summary>
    public Dictionary<string, object>? Context { get; }

    /// <summary>
    /// Initializes a new instance of the FileOperationException class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="context">Additional context information about the error</param>
    public FileOperationException(string message, Dictionary<string, object>? context = null) 
        : base(message)
    {
        Context = context;
    }

    /// <summary>
    /// Initializes a new instance of the FileOperationException class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="inner">The inner exception</param>
    /// <param name="context">Additional context information about the error</param>
    public FileOperationException(string message, Exception inner, Dictionary<string, object>? context = null) 
        : base(message, inner)
    {
        Context = context;
    }
} 





