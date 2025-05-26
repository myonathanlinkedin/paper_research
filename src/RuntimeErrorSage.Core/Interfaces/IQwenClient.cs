using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.LLM;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Interface for Qwen LLM client operations.
    /// </summary>
    public interface IQwenClient
    {
        /// <summary>
        /// Gets whether the client is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the client name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the client version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets whether the client is connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Connects to the Qwen service.
        /// </summary>
        Task ConnectAsync();

        /// <summary>
        /// Disconnects from the Qwen service.
        /// </summary>
        Task DisconnectAsync();

        /// <summary>
        /// Gets remediation suggestions for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation suggestions.</returns>
        Task<RemediationSuggestion> GetRemediationSuggestionsAsync(ErrorContext context);

        /// <summary>
        /// Analyzes an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The model response.</returns>
        Task<ModelResponse> AnalyzeErrorAsync(ErrorContext context);

        /// <summary>
        /// Validates a remediation suggestion.
        /// </summary>
        /// <param name="suggestion">The remediation suggestion.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The model response.</returns>
        Task<ModelResponse> ValidateSuggestionAsync(RemediationSuggestion suggestion, ErrorContext context);

        /// <summary>
        /// Explains a remediation suggestion.
        /// </summary>
        /// <param name="suggestion">The remediation suggestion.</param>
        /// <returns>The model response.</returns>
        Task<ModelResponse> ExplainSuggestionAsync(RemediationSuggestion suggestion);

        /// <summary>
        /// Gets the model metrics.
        /// </summary>
        /// <returns>The model metrics.</returns>
        Task<ModelMetrics> GetMetricsAsync();
    }
} 