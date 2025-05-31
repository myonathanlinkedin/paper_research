using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Validation;

namespace RuntimeErrorSage.Application.Remediation.Interfaces
{
    /// <summary>
    /// Interface for a system that manages remediation actions.
    /// </summary>
    public interface IRemediationActionSystem
    {
        /// <summary>
        /// Gets whether the system is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the system name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the system version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Creates a remediation action for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The created remediation action.</returns>
        Task<RemediationAction> CreateActionAsync(ErrorContext context);

        /// <summary>
        /// Executes a remediation action.
        /// </summary>
        /// <param name="action">The remediation action to execute.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The result of the execution.</returns>
        Task<RemediationResult> ExecuteActionAsync(RemediationAction action, ErrorContext context);

        /// <summary>
        /// Validates a remediation action.
        /// </summary>
        /// <param name="action">The remediation action to validate.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateActionAsync(RemediationAction action, ErrorContext context);

        /// <summary>
        /// Rolls back a remediation action.
        /// </summary>
        /// <param name="actionId">The ID of the remediation action to roll back.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The rollback status.</returns>
        Task<RuntimeErrorSage.Domain.Enums.RollbackStatus> RollbackActionAsync(string actionId, ErrorContext context);

        /// <summary>
        /// Gets the status of a remediation action.
        /// </summary>
        /// <param name="actionId">The ID of the remediation action.</param>
        /// <returns>The status of the remediation action.</returns>
        Task<RemediationStatusEnum> GetActionStatusAsync(string actionId);

        /// <summary>
        /// Creates a remediation plan for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The created remediation plan.</returns>
        Task<RemediationPlan> CreatePlanAsync(ErrorContext context);

        /// <summary>
        /// Executes a remediation plan.
        /// </summary>
        /// <param name="plan">The remediation plan to execute.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The result of the execution.</returns>
        Task<RemediationResult> ExecutePlanAsync(RemediationPlan plan, ErrorContext context);
    }
} 