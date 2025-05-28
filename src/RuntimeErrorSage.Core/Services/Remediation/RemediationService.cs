using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Services.Remediation
{
    /// <summary>
    /// Service for handling remediation actions.
    /// </summary>
    public class RemediationService : IRemediationService
    {
        private readonly IRemediationActionExecutor _actionExecutor;
        private readonly IRemediationValidator _validator;
        private readonly IRemediationRollbackManager _rollbackManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationService"/> class.
        /// </summary>
        public RemediationService(
            IRemediationActionExecutor actionExecutor,
            IRemediationValidator validator,
            IRemediationRollbackManager rollbackManager)
        {
            _actionExecutor = actionExecutor;
            _validator = validator;
            _rollbackManager = rollbackManager;
        }

        /// <summary>
        /// Executes a remediation action.
        /// </summary>
        public async Task<RemediationResult> ExecuteActionAsync(RemediationAction action)
        {
            var result = new RemediationResult
            {
                ActionId = action.ActionId,
                Status = RemediationStatusEnum.InProgress,
                Timestamp = DateTime.UtcNow
            };

            try
            {
                // Validate the action
                var validationResult = await _validator.ValidateActionAsync(action);
                if (!validationResult.IsValid)
                {
                    result.Status = RemediationStatusEnum.Failed;
                    result.ErrorMessage = validationResult.ErrorMessage;
                    return result;
                }

                // Execute the action
                var executionResult = await _actionExecutor.ExecuteActionAsync(action);
                result.Success = executionResult.Success;
                result.Status = executionResult.Success ? RemediationStatusEnum.Completed : RemediationStatusEnum.Failed;
                result.ErrorMessage = executionResult.ErrorMessage;
                result.Metrics = executionResult.Metrics;
            }
            catch (Exception ex)
            {
                result.Status = RemediationStatusEnum.Failed;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Validates a remediation action.
        /// </summary>
        public async Task<ValidationResult> ValidateActionAsync(RemediationAction action)
        {
            return await _validator.ValidateActionAsync(action);
        }

        /// <summary>
        /// Rolls back a remediation action.
        /// </summary>
        public async Task<RollbackStatus> RollbackActionAsync(string actionId)
        {
            return await _rollbackManager.RollbackActionAsync(actionId);
        }

        /// <summary>
        /// Gets the status of a remediation action.
        /// </summary>
        public async Task<RemediationResult> GetActionStatusAsync(string actionId)
        {
            return await _actionExecutor.GetActionStatusAsync(actionId);
        }

        public async Task<RemediationMetrics> GetMetricsAsync(string remediationId)
        {
            if (string.IsNullOrEmpty(remediationId))
                throw new ArgumentException("Remediation ID cannot be null or empty.", nameof(remediationId));

            // Implementation will be added
            return await Task.FromResult(new RemediationMetrics(remediationId));
        }
    }
} 