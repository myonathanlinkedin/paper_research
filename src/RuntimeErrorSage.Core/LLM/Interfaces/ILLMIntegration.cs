using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Model.Interfaces;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.LLM;
using RuntimeErrorSage.Model.Models.Remediation;
using RuntimeErrorSage.Model.Models.Validation;

namespace RuntimeErrorSage.Model.LLM.Interfaces
{
    /// <summary>
    /// Interface for LLM integration.
    /// </summary>
    public interface ILLMIntegration
    {
        /// <summary>
        /// Gets whether the LLM integration is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the LLM model name.
        /// </summary>
        string ModelName { get; }

        /// <summary>
        /// Gets the LLM model version.
        /// </summary>
        string ModelVersion { get; }

        /// <summary>
        /// Gets remediation suggestions for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>A list of remediation suggestions.</returns>
        Task<RemediationSuggestion> GetRemediationSuggestionsAsync(ErrorContext context);

        /// <summary>
        /// Validates a remediation suggestion.
        /// </summary>
        /// <param name="suggestion">The remediation suggestion.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The validation result.</returns>
        Task<ModelResponse> ValidateSuggestionAsync(RemediationSuggestion suggestion, ErrorContext context);

        /// <summary>
        /// Analyzes an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The analysis result.</returns>
        Task<ModelResponse> AnalyzeErrorAsync(ErrorContext context);

        /// <summary>
        /// Explains a remediation suggestion.
        /// </summary>
        /// <param name="suggestion">The remediation suggestion.</param>
        /// <returns>The explanation.</returns>
        Task<ModelResponse> ExplainSuggestionAsync(RemediationSuggestion suggestion);

        Task<List<RemediationSuggestion>> GenerateRemediationSuggestionsAsync(RuntimeError error, RemediationAnalysis analysis);
        Task<RemediationValidationResult> ValidateRemediationAsync(RemediationSuggestion suggestion, ErrorContext context);
    }
} 
