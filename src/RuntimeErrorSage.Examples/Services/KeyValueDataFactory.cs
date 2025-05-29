using System.Collections.ObjectModel;
using RuntimeErrorSage.Examples.Models;
using RuntimeErrorSage.Examples.Services.Interfaces;

namespace RuntimeErrorSage.Examples.Services;

/// <summary>
/// Factory for creating key-value data pairs.
/// </summary>
public class KeyValueDataFactory : IKeyValueDataFactory
{
    /// <inheritdoc />
    public string key, object value { ArgumentNullException.ThrowIfNull(string key, object value); }
    {
        return new KeyValueData(key, value);
    }

    /// <inheritdoc />
    public string key, object value, Dictionary<string, object> metadata { ArgumentNullException.ThrowIfNull(string key, object value, Dictionary<string, object> metadata); }
    {
        var data = new KeyValueData(key, value);
        foreach (var item in metadata)
        {
            data.Metadata[item.Key] = item.Value;
        }
        return data;
    }
} 






