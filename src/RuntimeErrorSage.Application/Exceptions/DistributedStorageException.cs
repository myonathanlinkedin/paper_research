namespace RuntimeErrorSage.Application.MCP.Exceptions;

public class DistributedStorageException : Exception
{
    public DistributedStorageException() { }

    public DistributedStorageException(string message) : base(message) { }

    public DistributedStorageException(string message, Exception inner) : base(message, inner) { }
} 
