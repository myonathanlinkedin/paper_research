using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Extensions;

/// <summary>
/// Extension methods for Dictionary to simplify common operations.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Gets the value associated with the specified key if it exists;
    /// otherwise, adds a new value created by the valueFactory and returns it.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to get or add the value to.</param>
    /// <param name="key">The key of the value to get or add.</param>
    /// <param name="valueFactory">The function used to generate a value if the key does not exist.</param>
    /// <returns>The value for the key. This will be either the existing value if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> valueFactory) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(valueFactory);
        ArgumentNullException.ThrowIfNull(key);

        if (!dictionary.TryGetValue(key, out var value))
        {
            value = valueFactory(key);
            dictionary[key] = value;
        }

        return value;
    }

    /// <summary>
    /// Gets the value associated with the specified key if it exists;
    /// otherwise, adds the specified value and returns it.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to get or add the value to.</param>
    /// <param name="key">The key of the value to get or add.</param>
    /// <param name="value">The value to add if the key does not exist.</param>
    /// <returns>The value for the key. This will be either the existing value if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        TValue value) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(key);

        if (!dictionary.TryGetValue(key, out var existingValue))
        {
            existingValue = value;
            dictionary[key] = existingValue;
        }

        return existingValue;
    }

    /// <summary>
    /// Gets a value from a dictionary with a null-safe operation.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="defaultValue">Default value if key not found or dictionary is null.</param>
    /// <returns>The value or default if not found.</returns>
    public static TValue GetValueOrDefault<TKey, TValue>(
        this Dictionary<TKey, TValue>? dictionary,
        TKey? key,
        TValue defaultValue = default) where TKey : notnull
    {
        if (dictionary == null || key == null || !dictionary.TryGetValue(key, out var value))
        {
            return defaultValue;
        }

        return value;
    }

    /// <summary>
    /// Adds a value to a dictionary, creating the dictionary if it's null.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public static void AddSafely<TKey, TValue>(
        this Dictionary<TKey, TValue>? dictionary,
        TKey? key,
        TValue value) where TKey : notnull
    {
        if (dictionary == null)
        {
            dictionary = new Dictionary<TKey, TValue>();
        }
        
        if (key != null)
        {
            dictionary[key] = value;
        }
    }
    
    /// <summary>
    /// Gets a numeric value from a dictionary with a null-safe operation.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="defaultValue">Default value if key not found or value can't be converted.</param>
    /// <returns>The value as double or default if not found or not convertible.</returns>
    public static double GetNumericValueOrDefault<TKey>(
        this Dictionary<TKey, object>? dictionary,
        TKey? key,
        double defaultValue = 0) where TKey : notnull
    {
        if (dictionary == null || key == null || !dictionary.TryGetValue(key, out var value))
        {
            return defaultValue;
        }

        if (value is double doubleValue)
        {
            return doubleValue;
        }
        
        if (value is int intValue)
        {
            return intValue;
        }
        
        if (value is float floatValue)
        {
            return floatValue;
        }
        
        if (value != null && double.TryParse(value.ToString(), out var parsedValue))
        {
            return parsedValue;
        }

        return defaultValue;
    }
} 
