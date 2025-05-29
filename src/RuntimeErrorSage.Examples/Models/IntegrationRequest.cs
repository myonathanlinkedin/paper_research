namespace RuntimeErrorSage.Examples.Models;

/// <summary>
/// Represents a request for external service integration
/// </summary>
public class IntegrationRequest
{
    /// <summary>
    /// The URL of the external service to integrate with
    /// </summary>
    public string? ServiceUrl { get; set; }

    /// <summary>
    /// The data payload to send to the service
    /// </summary>
    public string? Payload { get; set; }

    /// <summary>
    /// The HTTP method to use for the service call (e.g., GET, POST)
    /// </summary>
    public string? Method { get; set; }

    /// <summary>
    /// The timeout for the service call in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; }
} 

