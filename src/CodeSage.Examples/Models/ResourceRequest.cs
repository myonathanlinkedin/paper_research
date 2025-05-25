namespace CodeSage.Examples.Models;

/// <summary>
/// Represents a request for resource allocation
/// </summary>
public class ResourceRequest
{
    /// <summary>
    /// The type of resource to allocate
    /// </summary>
    public string? ResourceType { get; set; }

    /// <summary>
    /// The quantity of resources to allocate
    /// </summary>
    public int RequestedAmount { get; set; }

    /// <summary>
    /// Optional priority level for the resource allocation (1 = lowest, 5 = highest)
    /// </summary>
    [System.ComponentModel.DataAnnotations.Range(1, 5, ErrorMessage = "Priority must be between 1 and 5")]
    public int? Priority { get; set; }
} 