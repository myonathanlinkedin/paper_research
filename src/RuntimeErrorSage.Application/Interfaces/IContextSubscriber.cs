using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;

namespace RuntimeErrorSage.Application.Context.Interfaces
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

