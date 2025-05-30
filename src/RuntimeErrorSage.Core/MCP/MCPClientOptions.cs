using System;

namespace RuntimeErrorSage.Core.MCP;

/// <summary>
/// Options for configuring the Model Control Protocol client.
/// </summary>
public class MCPClientOptions
{
    /// <summary>
    /// Gets or sets the server URL.
    /// </summary>
    public string ServerUrl { get; set; } = "http://localhost:5000";

    /// <summary>
    /// Gets or sets the API key.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the connection timeout in seconds.
    /// </summary>
    public int ConnectionTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Gets or sets the request timeout in seconds.
    /// </summary>
    public int RequestTimeoutSeconds { get; set; } = 60;

    /// <summary>
    /// Gets or sets the maximum number of retry attempts.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Gets or sets the retry delay in milliseconds.
    /// </summary>
    public int RetryDelayMilliseconds { get; set; } = 1000;

    /// <summary>
    /// Gets or sets a value indicating whether to use secure connection.
    /// </summary>
    public bool UseSecureConnection { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum cache size.
    /// </summary>
    public int MaxCacheSize { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the cache expiration time in minutes.
    /// </summary>
    public int CacheExpirationMinutes { get; set; } = 60;

    /// <summary>
    /// Validates the options.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the options are invalid.</exception>
    public void Validate()
    {
        if (string.IsNullOrEmpty(ServerUrl))
            throw new ArgumentException("Server URL cannot be null or empty.", nameof(ServerUrl));

        if (string.IsNullOrEmpty(ApiKey))
            throw new ArgumentException("API key cannot be null or empty.", nameof(ApiKey));

        if (ConnectionTimeoutSeconds <= 0)
            throw new ArgumentException("Connection timeout must be greater than 0.", nameof(ConnectionTimeoutSeconds));

        if (RequestTimeoutSeconds <= 0)
            throw new ArgumentException("Request timeout must be greater than 0.", nameof(RequestTimeoutSeconds));

        if (MaxRetryAttempts < 0)
            throw new ArgumentException("Max retry attempts cannot be negative.", nameof(MaxRetryAttempts));

        if (RetryDelayMilliseconds <= 0)
            throw new ArgumentException("Retry delay must be greater than 0.", nameof(RetryDelayMilliseconds));

        if (MaxCacheSize <= 0)
            throw new ArgumentException("Max cache size must be greater than 0.", nameof(MaxCacheSize));

        if (CacheExpirationMinutes <= 0)
            throw new ArgumentException("Cache expiration minutes must be greater than 0.", nameof(CacheExpirationMinutes));
    }
} 
