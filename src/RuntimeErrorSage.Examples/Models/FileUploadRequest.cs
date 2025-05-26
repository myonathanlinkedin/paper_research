namespace RuntimeErrorSage.Examples.Models;

/// <summary>
/// Represents a request for file upload operations
/// </summary>
public class FileUploadRequest
{
    /// <summary>
    /// The name of the file to upload
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// The size of the file in bytes
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// The MIME type of the file
    /// </summary>
    public string? ContentType { get; set; }
} 
