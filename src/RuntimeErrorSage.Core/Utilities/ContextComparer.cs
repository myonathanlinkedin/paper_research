using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Utilities
{
    /// <summary>
    /// Utility class for comparing error contexts and patterns.
    /// </summary>
    public static class ContextComparer
    {
        private static readonly string[] RelevantKeys = new[] 
        { 
            "DatabaseName", 
            "FilePath", 
            "ServiceEndpoint" 
        };

        /// <summary>
        /// Compares additional context dictionaries for similarity.
        /// </summary>
        /// <param name="patternContext">The pattern context to compare</param>
        /// <param name="currentContext">The current context to compare</param>
        /// <returns>True if the contexts are similar, false otherwise</returns>
        public static bool CompareAdditionalContext(
            Dictionary<string, object> patternContext,
            Dictionary<string, string> currentContext)
        {
            if (patternContext == null || currentContext == null)
                return false;

            return RelevantKeys.All(key =>
                patternContext.TryGetValue(key, out var patternValue) &&
                currentContext.TryGetValue(key, out var currentValue) &&
                patternValue?.ToString() == currentValue);
        }
    }
} 
