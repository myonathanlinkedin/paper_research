using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Interface for pattern recognition functionality.
    /// </summary>
    public interface IPatternRecognition
    {
        /// <summary>
        /// Initializes the pattern recognition system.
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Detects patterns in a list of error contexts.
        /// </summary>
        /// <param name="contexts">The list of error contexts to analyze.</param>
        /// <param name="serviceName">The name of the service being analyzed.</param>
        /// <returns>A list of detected error patterns.</returns>
        Task<List<ErrorPattern>> DetectPatternsAsync(List<ErrorContext> contexts, string serviceName);

        /// <summary>
        /// Finds a matching pattern for a given error context.
        /// </summary>
        /// <param name="context">The error context to match.</param>
        /// <param name="serviceName">The name of the service being analyzed.</param>
        /// <returns>The matching error pattern, if found; otherwise, null.</returns>
        Task<ErrorPattern?> FindMatchingPatternAsync(ErrorContext context, string serviceName);
    }
} 
