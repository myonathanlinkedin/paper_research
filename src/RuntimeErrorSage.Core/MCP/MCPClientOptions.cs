using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.MCP;

/// <summary>
/// Options for the MCP client.
/// </summary>
public class MCPClientOptions
{
    /// <summary>
    /// Gets or sets the base URL.
    /// </summary>
    public string BaseUrl { get; }

    /// <summary>
    /// Gets or sets the API key.
    /// </summary>
    public string ApiKey { get; }

    /// <summary>
    /// Gets or sets the timeout in seconds.
    /// </summary>
    public int TimeoutSeconds { get; } = 30;

    /// <summary>
    /// Gets or sets the retry count.
    /// </summary>
    public int RetryCount { get; } = 3;

    /// <summary>
    /// Gets or sets the retry delay in seconds.
    /// </summary>
    public int RetryDelaySeconds { get; } = 1;

    /// <summary>
    /// Validates the options.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the options are invalid.</exception>
    public void Validate()
    {
        if (string.IsNullOrEmpty(BaseUrl))
            throw new ArgumentException("Base URL cannot be null or empty.", nameof(BaseUrl));

        if (string.IsNullOrEmpty(ApiKey))
            throw new ArgumentException("API key cannot be null or empty.", nameof(ApiKey));

        if (TimeoutSeconds <= 0)
            throw new ArgumentException("Timeout must be greater than 0.", nameof(TimeoutSeconds));

        if (RetryCount < 0)
            throw new ArgumentException("Retry count cannot be negative.", nameof(RetryCount));

        if (RetryDelaySeconds <= 0)
            throw new ArgumentException("Retry delay must be greater than 0.", nameof(RetryDelaySeconds));
    }
} 





