namespace RuntimeErrorSage.Examples.Models;

/// <summary>
/// Context information for database-related errors
/// </summary>
public class DatabaseErrorContext
{
    /// <summary>
    /// The database operation that failed
    /// </summary>
    public string? Operation { get; set; }

    /// <summary>
    /// The table or collection affected
    /// </summary>
    public string? Table { get; set; }

    /// <summary>
    /// The query or command that failed
    /// </summary>
    public string? Query { get; set; }

    /// <summary>
    /// Additional error context
    /// </summary>
    public Dictionary<string, object>? Context { get; set; }
} 