using System.Diagnostics;

namespace RuntimeErrorSage.Application.Extensions
{
    /// <summary>
    /// Extension methods for Activity class.
    /// </summary>
    public static class ActivityExtensions
    {
        /// <summary>
        /// Gets a tag value from the activity.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="key">The tag key.</param>
        /// <returns>The tag value or null if not found.</returns>
        public static string GetTag(this Activity activity, string key)
        {
            if (activity == null)
                return null;

            // Look for the tag in activity tags
            foreach (var tag in activity.Tags)
            {
                if (tag.Key == key)
                {
                    return tag.Value?.ToString();
                }
            }

            // Not found
            return null;
        }
    }
} 