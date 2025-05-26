using System.Threading.Tasks;
using CodeSage.Core.Models;
using CodeSage.Core.Models.Error;

namespace CodeSage.Core.Interfaces
{
    /// <summary>
    /// Defines the interface for context subscribers.
    /// </summary>
    public interface IContextSubscriber
    {
        /// <summary>
        /// Handles a new context update.
        /// </summary>
        /// <param name="context">The error context</param>
        /// <param name="analysis">The analysis result</param>
        Task HandleContextUpdateAsync(ErrorContext context, ErrorAnalysisResult analysis);
    }
} 