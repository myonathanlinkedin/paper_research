using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace RuntimeErrorSage.Examples.Models;

/// <summary>
/// Represents a request for data processing operations
/// </summary>
public class ProcessRequest : BaseRequest
{
    /// <summary>
    /// The data to be processed
    /// </summary>
    [Required(ErrorMessage = "Data is required")]
    [MinLength(1, ErrorMessage = "Data cannot be empty")]
    [MaxLength(10000, ErrorMessage = "Data exceeds maximum length")]
    public string? Data { get; set; }

    /// <summary>
    /// The type of operation to perform
    /// </summary>
    [Required(ErrorMessage = "Operation type is required")]
    [RegularExpression(@"^(CREATE|UPDATE|DELETE|QUERY)$", 
        ErrorMessage = "Operation must be one of: CREATE, UPDATE, DELETE, QUERY")]
    public string? Operation { get; set; }

    /// <summary>
    /// Optional priority level for the operation
    /// </summary>
    [Range(1, 5, ErrorMessage = "Priority must be between 1 and 5")]
    public int? Priority { get; set; }

    /// <summary>
    /// Optional timeout in seconds
    /// </summary>
    [Range(1, 300, ErrorMessage = "Timeout must be between 1 and 300 seconds")]
    public int? TimeoutSeconds { get; set; }
} 





