namespace RuntimeErrorSage.Examples.Exceptions;

/// <summary>
/// Exception thrown when resource allocation fails
/// </summary>
public class ResourceAllocationException : Exception
{
    /// <summary>
    /// Additional context information about the error
    /// </summary>
    public Dictionary<string, object>? Context { get; }

    /// <summary>
    /// Initializes a new instance of the ResourceAllocationException class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="context">Additional context information about the error</param>
    public ResourceAllocationException(string message, Dictionary<string, object>? context = null) 
        : base(message)
    {
        Context = context;
    }

    /// <summary>
    /// Initializes a new instance of the ResourceAllocationException class
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="inner">The inner exception</param>
    /// <param name="context">Additional context information about the error</param>
    public ResourceAllocationException(string message, Exception inner, Dictionary<string, object>? context = null) 
        : base(message, inner)
    {
        Context = context;
    }
} 
