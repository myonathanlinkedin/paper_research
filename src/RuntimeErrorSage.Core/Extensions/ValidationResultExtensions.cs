using System;
using RuntimeErrorSage.Domain.Models.Validation;

namespace RuntimeErrorSage.Core.Extensions
{
    /// <summary>
    /// Extension methods for ValidationResult to maintain compatibility with code that expects IsSuccessful property
    /// </summary>
    public static class ValidationResultExtensions
    {
        /// <summary>
        /// Gets whether the validation result is successful.
        /// </summary>
        /// <param name="result">The validation result.</param>
        /// <returns>True if the result is valid, false otherwise.</returns>
        public static bool IsSuccessful(this ValidationResult result)
        {
            if (result == null)
            {
                return false;
            }
            
            return result.IsValid;
        }
        
        /// <summary>
        /// Gets whether the validation result is from cache.
        /// </summary>
        /// <param name="result">The validation result.</param>
        /// <returns>True if the result is from cache, false otherwise.</returns>
        public static bool IsFromCache(this ValidationResult result)
        {
            if (result == null)
            {
                return false;
            }
            
            // Check if the result contains any metadata indicating it's from cache
            return result.Details.ContainsKey("FromCache") && 
                   result.Details["FromCache"] is bool fromCache && 
                   fromCache;
        }
    }
} 