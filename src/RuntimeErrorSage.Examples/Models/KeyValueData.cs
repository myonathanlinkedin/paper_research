namespace RuntimeErrorSage.Examples.Models;

/// <summary>
/// Represents a key-value data pair with metadata.
/// </summary>
public class KeyValueData
{
    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// Gets or sets the metadata associated with this key-value pair.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; }

    /// <summary>
    /// Initializes a new instance of the KeyValueData class.
    /// </summary>
    public KeyValueData()
    {
        Metadata = new Dictionary<string, object>();
    }

    /// <summary>
    /// Initializes a new instance of the KeyValueData class with the specified key and value.
    /// </summary>
    public KeyValueData(string key, object value)
    {
        Key = key;
        Value = value;
        Metadata = new Dictionary<string, object>();
    }
} 