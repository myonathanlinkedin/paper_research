using RuntimeErrorSage.Examples.Models;

namespace RuntimeErrorSage.Examples.Services.Interfaces;

/// <summary>
/// Factory interface for creating key-value data pairs.
/// </summary>
public interface IKeyValueDataFactory
{
    /// <summary>
    /// Creates a new key-value data pair.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns>A new KeyValueData instance.</returns>
    KeyValueData Create(string key, object value);

    /// <summary>
    /// Creates a new key-value data pair with metadata.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="metadata">The metadata.</param>
    /// <returns>A new KeyValueData instance with metadata.</returns>
    KeyValueData CreateWithMetadata(string key, object value, Dictionary<string, object> metadata);
} 