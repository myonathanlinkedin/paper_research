using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Context;
using RuntimeErrorSage.Domain.Models.MCP;
using System.ComponentModel.DataAnnotations;

namespace RuntimeErrorSage.Application.Protocols
{
    /// <summary>
    /// Interface for the Model Context Protocol which manages interaction between runtime error analysis and context gathering.
    /// </summary>
    public interface IModelContextProtocol
    {
        /// <summary>
        /// Analyzes the runtime context and builds a graph-based representation.
        /// </summary>
        /// <param name="context">The runtime context to analyze.</param>
        /// <returns>The result of the context analysis.</returns>
        Task<ContextAnalysisResult> AnalyzeContextAsync(RuntimeContext context);

        /// <summary>
        /// Updates the context model based on new runtime information.
        /// </summary>
        /// <param name="contextId">The ID of the context to update.</param>
        /// <param name="update">The update information.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateContextModelAsync(string contextId, RuntimeUpdate update);

        /// <summary>
        /// Validates the context against defined rules and constraints.
        /// </summary>
        /// <param name="contextId">The ID of the context to validate.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateContextAsync(string contextId);
    }
} 