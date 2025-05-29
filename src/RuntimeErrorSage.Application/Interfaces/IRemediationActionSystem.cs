using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Interfaces
{
    /// <summary>
    /// Interface for managing remediation actions.
    /// </summary>
    public interface IRemediationActionSystem
    {
        /// <summary>
        /// Gets whether the action system is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the action system name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the action system version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets remediation suggestions for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation suggestions.</returns>
        Task<RemediationSuggestion> GetRemediationSuggestionsAsync(ErrorContext context);

        /// <summary>
        /// Validates a remediation suggestion.
        /// </summary>
        /// <param name="suggestion">The remediation suggestion.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateSuggestionAsync(RemediationSuggestion suggestion, ErrorContext context);

        /// <summary>
        /// Executes a remediation suggestion.
        /// </summary>
        /// <param name="suggestion">The remediation suggestion.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation result.</returns>
        Task<RemediationResult> ExecuteSuggestionAsync(RemediationSuggestion suggestion, ErrorContext context);

        /// <summary>
        /// Gets the impact of a remediation suggestion.
        /// </summary>
        /// <param name="suggestion">The remediation suggestion.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation impact.</returns>
        Task<RemediationImpact> GetSuggestionImpactAsync(RemediationSuggestion suggestion, ErrorContext context);

        /// <summary>
        /// Gets the risk assessment for a remediation suggestion.
        /// </summary>
        /// <param name="suggestion">The remediation suggestion.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The risk assessment.</returns>
        Task<RiskAssessment> GetSuggestionRiskAsync(RemediationSuggestion suggestion, ErrorContext context);
    }
} 


