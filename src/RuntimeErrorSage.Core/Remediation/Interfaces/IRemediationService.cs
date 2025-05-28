using RuntimeErrorSage.Core.Interfaces;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Options;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Interface for managing remediation operations.
    /// </summary>
    public interface IRemediationService
    {
        /// <summary>
        /// Gets whether the service is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the service name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the service version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Applies remediation to an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation result.</returns>
        Task<RemediationResult> ApplyRemediationAsync(ErrorContext context);

        /// <summary>
        /// Creates a remediation plan.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation plan.</returns>
        Task<RemediationPlan> CreatePlanAsync(ErrorContext context);

        /// <summary>
        /// Validates a remediation plan.
        /// </summary>
        /// <param name="plan">The remediation plan.</param>
        /// <returns>True if the plan is valid, false otherwise.</returns>
        Task<bool> ValidatePlanAsync(RemediationPlan plan);

        /// <summary>
        /// Gets the status of a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation ID.</param>
        /// <returns>The remediation status.</returns>
        Task<RemediationStatusEnum> GetStatusAsync(string remediationId);

        /// <summary>
        /// Gets metrics for a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation ID.</param>
        /// <returns>The remediation metrics.</returns>
        Task<RemediationMetrics> GetMetricsAsync(string remediationId);

        /// <summary>
        /// Gets remediation suggestions for an error.
        /// </summary>
        /// <param name="errorContext">The error context.</param>
        /// <returns>A remediation suggestion.</returns>
        Task<RemediationSuggestion> GetRemediationSuggestionsAsync(ErrorContext errorContext);

        /// <summary>
        /// Validates a remediation suggestion.
        /// </summary>
        /// <param name="suggestion">The suggestion to validate.</param>
        /// <param name="errorContext">The error context.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext);

        /// <summary>
        /// Executes a remediation suggestion.
        /// </summary>
        /// <param name="suggestion">The suggestion to execute.</param>
        /// <param name="errorContext">The error context.</param>
        /// <returns>The remediation result.</returns>
        Task<RemediationResult> ExecuteSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext);

        /// <summary>
        /// Gets the impact assessment for a remediation suggestion.
        /// </summary>
        /// <param name="suggestion">The suggestion to assess.</param>
        /// <param name="errorContext">The error context.</param>
        /// <returns>The remediation impact.</returns>
        Task<RemediationImpact> GetSuggestionImpactAsync(RemediationSuggestion suggestion, ErrorContext errorContext);

        /// <summary>
        /// Executes a remediation action.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>The result of the action execution.</returns>
        Task<RemediationResult> ExecuteActionAsync(RemediationAction action);

        /// <summary>
        /// Validates a remediation action.
        /// </summary>
        /// <param name="action">The action to validate.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateActionAsync(RemediationAction action);

        /// <summary>
        /// Rolls back a remediation action.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <returns>The rollback status.</returns>
        Task<RollbackStatus> RollbackActionAsync(string actionId);

        /// <summary>
        /// Gets the status of a remediation action.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <returns>The action status.</returns>
        Task<RemediationResult> GetActionStatusAsync(string actionId);

        /// <summary>
        /// Registers a remediation strategy.
        /// </summary>
        /// <param name="strategy">The strategy to register.</param>
        void RegisterStrategy(IRemediationStrategy strategy);

        /// <summary>
        /// Configures the service with specific options.
        /// </summary>
        /// <param name="options">The configuration options.</param>
        void Configure(RuntimeErrorSageOptions options);

        /// <summary>
        /// Remediates an error based on the analysis result.
        /// </summary>
        /// <param name="analysis">The error analysis result.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation result.</returns>
        Task<RemediationResult> RemediateAsync(ErrorAnalysisResult analysis, ErrorContext context);
    }
} 

