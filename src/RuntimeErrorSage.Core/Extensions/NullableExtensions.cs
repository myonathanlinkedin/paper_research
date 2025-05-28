using System;

namespace RuntimeErrorSage.Core.Extensions
{
    /// <summary>
    /// Extension methods for nullable types.
    /// </summary>
    public static class NullableExtensions
    {
        /// <summary>
        /// Gets the value or default for a nullable DateTime.
        /// </summary>
        /// <param name="value">The nullable DateTime.</param>
        /// <param name="defaultValue">The default value to use if the value is null.</param>
        /// <returns>The DateTime value or default.</returns>
        public static DateTime GetValueOrDefault(this DateTime? value, DateTime defaultValue = default)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Gets the value or default for a nullable double.
        /// </summary>
        /// <param name="value">The nullable double.</param>
        /// <param name="defaultValue">The default value to use if the value is null.</param>
        /// <returns>The double value or default.</returns>
        public static double GetValueOrDefault(this double? value, double defaultValue = 0)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Gets the value or default for a nullable int.
        /// </summary>
        /// <param name="value">The nullable int.</param>
        /// <param name="defaultValue">The default value to use if the value is null.</param>
        /// <returns>The int value or default.</returns>
        public static int GetValueOrDefault(this int? value, int defaultValue = 0)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Gets the value or default for a nullable bool.
        /// </summary>
        /// <param name="value">The nullable bool.</param>
        /// <param name="defaultValue">The default value to use if the value is null.</param>
        /// <returns>The bool value or default.</returns>
        public static bool GetValueOrDefault(this bool? value, bool defaultValue = false)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Gets the value or default for a nullable Guid.
        /// </summary>
        /// <param name="value">The nullable Guid.</param>
        /// <param name="defaultValue">The default value to use if the value is null.</param>
        /// <returns>The Guid value or default.</returns>
        public static Guid GetValueOrDefault(this Guid? value, Guid defaultValue = default)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Converts a nullable value to its string representation or a default string.
        /// </summary>
        /// <typeparam name="T">The type of the nullable value.</typeparam>
        /// <param name="value">The nullable value.</param>
        /// <param name="defaultValue">The default string to use if the value is null.</param>
        /// <returns>The string representation of the value or the default string.</returns>
        public static string ToStringOrDefault<T>(this T? value, string defaultValue = "") where T : struct
        {
            return value?.ToString() ?? defaultValue;
        }
    }
} 
