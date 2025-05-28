using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Execution;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    /// <summary>
    /// Interface for executing remediation operations.
    /// </summary>
    public interface IRemediationExecutor
    {
        /// <summary>
        /// Gets whether the executor is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the executor name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the executor version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Executes a remediation strategy.
        /// </summary>
        /// <param name="strategy">The remediation strategy.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation result.</returns>
        Task<RemediationResult> ExecuteStrategyAsync(IRemediationStrategy strategy, ErrorContext context);

        /// <summary>
        /// Executes a remediation plan.
        /// </summary>
        /// <param name="plan">The remediation plan.</param>
        /// <returns>The remediation result.</returns>
        Task<RemediationResult> ExecutePlanAsync(RemediationPlan plan);

        /// <summary>
        /// Rolls back a remediation.
        /// </summary>
        /// <param name="result">The remediation result to roll back.</param>
        /// <returns>The rollback result.</returns>
        Task<RemediationResult> RollbackAsync(RemediationResult result);

        /// <summary>
        /// Gets the status of a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation ID.</param>
        /// <returns>The remediation status.</returns>
        Task<RemediationStatusEnum> GetRemediationStatusAsync(string remediationId);

        /// <summary>
        /// Gets the execution history for a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation ID.</param>
        /// <returns>The execution history.</returns>
        Task<RemediationExecution> GetExecutionHistoryAsync(string remediationId);

        /// <summary>
        /// Cancels a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation ID.</param>
        /// <returns>True if the operation was cancelled, false otherwise.</returns>
        Task<bool> CancelRemediationAsync(string remediationId);

        /// <summary>
        /// Executes a remediation action based on the provided analysis and context.
        /// </summary>
        /// <param name="analysis">The error analysis result</param>
        /// <param name="context">The error context</param>
        /// <returns>The remediation execution result</returns>
        Task<RemediationExecution> ExecuteRemediationAsync(ErrorAnalysisResult analysis, ErrorContext context);

        /// <summary>
        /// Validates a remediation action before execution.
        /// </summary>
        /// <param name="analysis">The error analysis result</param>
        /// <param name="context">The error context</param>
        /// <returns>The validation result</returns>
        Task<RemediationValidationResult> ValidateRemediationAsync(ErrorAnalysisResult analysis, ErrorContext context);

        /// <summary>
        /// Gets the current execution metrics.
        /// </summary>
        /// <param name="remediationId">The remediation ID</param>
        /// <returns>The execution metrics</returns>
        Task<RemediationMetrics> GetExecutionMetricsAsync(string remediationId);

        /// <summary>
        /// Executes a remediation action.
        /// </summary>
        /// <param name="action">The remediation action to execute.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The execution result.</returns>
        Task<RemediationAction> ExecuteActionAsync(RemediationAction action, ErrorContext context);

        /// <summary>
        /// Validates a remediation action.
        /// </summary>
        /// <param name="action">The remediation action to validate.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateActionAsync(RemediationAction action, ErrorContext context);

        /// <summary>
        /// Gets the impact of a remediation action.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The impact assessment.</returns>
        Task<RemediationImpact> GetActionImpactAsync(RemediationAction action, ErrorContext context);

        /// <summary>
        /// Gets the risk assessment for a remediation action.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The risk assessment.</returns>
        Task<RiskAssessment> GetActionRiskAsync(RemediationAction action, ErrorContext context);

        /// <summary>
        /// Rolls back a remediation.
        /// </summary>
        /// <param name="remediationId">The remediation ID.</param>
        /// <returns>The remediation result.</returns>
        Task<RemediationResult> RollbackRemediationAsync(string remediationId);
    }
} 