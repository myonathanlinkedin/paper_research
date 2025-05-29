using System.Threading.Tasks;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.Context;

namespace RuntimeErrorSage.Model.Context.Interfaces
{
    /// <summary>
    /// Interface for providing error context.
    /// </summary>
    public interface IContextProvider
    {
        /// <summary>
        /// Gets whether the context provider is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the context provider name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the context provider version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the error context.
        /// </summary>
        /// <param name="errorId">The error ID.</param>
        /// <returns>The error context.</returns>
        Task<ErrorContext> GetContextAsync(string errorId);

        /// <summary>
        /// Gets the runtime context.
        /// </summary>
        /// <param name="errorId">The error ID.</param>
        /// <returns>The runtime context.</returns>
        Task<RuntimeContext> GetRuntimeContextAsync(string errorId);

        /// <summary>
        /// Gets the context metadata.
        /// </summary>
        /// <param name="errorId">The error ID.</param>
        /// <returns>The context metadata.</returns>
        Task<ContextMetadata> GetContextMetadataAsync(string errorId);

        /// <summary>
        /// Updates the error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        Task UpdateContextAsync(ErrorContext context);

        /// <summary>
        /// Updates the runtime context.
        /// </summary>
        /// <param name="context">The runtime context.</param>
        Task UpdateRuntimeContextAsync(RuntimeContext context);

        /// <summary>
        /// Updates the context metadata.
        /// </summary>
        /// <param name="metadata">The context metadata.</param>
        Task UpdateContextMetadataAsync(ContextMetadata metadata);
    }
} 
