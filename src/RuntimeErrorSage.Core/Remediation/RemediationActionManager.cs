using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Enums;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Domain.Models.Metrics;

namespace RuntimeErrorSage.Core.Remediation
{
    public class RemediationActionManager : IRemediationActionManager
    {
        private readonly ILogger<RemediationActionManager> _logger;
        private readonly IRemediationValidator _validator;
        private readonly IRemediationExecutor _executor;

        public RemediationActionManager(
            ILogger<RemediationActionManager> logger,
            IRemediationValidator validator,
            IRemediationExecutor executor)
        {
            _logger = logger;
            _validator = validator;
            _executor = executor;
        }

        /// <summary>
        /// Executes a remediation action.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>The result of the execution.</returns>
        public async Task<RemediationResult> ExecuteActionAsync(RemediationAction action)
        {
            _logger.LogInformation("Executing remediation action {ActionId}: {ActionName}", action.Id, action.Name);

            try
            {
                // Validate the action before execution
                var validationResult = await ValidateActionAsync(action);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Remediation action {ActionId} validation failed: {ValidationMessage}", 
                        action.Id, validationResult.Message);
                    
                    var actionContext = action.Context;
                    var resultObj = new RemediationResult(actionContext, RemediationStatusEnum.ValidationFailed, validationResult.Message, string.Empty)
                    {
                        IsSuccessful = false,
                        ActionId = action.Id,
                        ExecutionId = Guid.NewGuid().ToString(),
                        Timestamp = DateTime.UtcNow
                    };
                    
                    if (validationResult.Errors.Count > 0)
                    {
                        resultObj.Errors.AddRange(validationResult.Errors);
                    }
                    
                    return resultObj;
                }

                // Execute the action
                var context = action.Context;
                var result = await _executor.ExecuteActionAsync(action, context);

                _logger.LogInformation("Remediation action {ActionId} executed with result: {Status}", 
                    action.Id, result.Status);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing remediation action {ActionId}: {ErrorMessage}", 
                    action.Id, ex.Message);
                
                var context = action.Context;
                return new RemediationResult(context, RemediationStatusEnum.Failed, ex.Message, ex.StackTrace)
                {
                    IsSuccessful = false,
                    ActionId = action.Id,
                    ExecutionId = Guid.NewGuid().ToString(),
                    Error = ex.ToString(),
                    Exception = ex,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Validates a remediation action.
        /// </summary>
        /// <param name="action">The action to validate.</param>
        /// <returns>The validation result.</returns>
        public async Task<Domain.Models.Validation.ValidationResult> ValidateActionAsync(RemediationAction action)
        {
            _logger.LogInformation("Validating remediation action {ActionId}: {ActionName}", action.Id, action.Name);

            try
            {
                var result = await _validator.ValidateActionAsync(action, action.Context);
                _logger.LogInformation("Validation for action {ActionId} completed. IsValid: {IsValid}", action.Id, result.IsValid);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating remediation action {ActionId}: {ErrorMessage}", action.Id, ex.Message);
                
                // Create a validation context for the exception
                var validationContext = new ValidationContext();
                validationContext.SetTarget(action);
                
                var result = new Domain.Models.Validation.ValidationResult(validationContext)
                {
                    IsValid = false,
                    Message = $"Validation error: {ex.Message}"
                };
                
                // Add validation error
                result.AddError(ex.Message);
                result.AddMessage($"Validation exception: {ex.Message}");
                
                return result;
            }
        }

        /// <summary>
        /// Rolls back a remediation action.
        /// </summary>
        /// <param name="actionId">The ID of the action to roll back.</param>
        /// <returns>The rollback status.</returns>
        public async Task<RuntimeErrorSage.Domain.Enums.RollbackStatus> RollbackActionAsync(string actionId)
        {
            _logger.LogInformation("Rolling back remediation action {ActionId}", actionId);

            try
            {
                // Find the action result
                var actionResult = await GetActionStatusAsync(actionId);
                if (actionResult == null)
                {
                    _logger.LogWarning("Action {ActionId} not found for rollback", actionId);
                    return RuntimeErrorSage.Domain.Enums.RollbackStatus.Failed;
                }

                // Initialize metrics
                var metrics = new ExecutionMetrics
                {
                    ActionId = actionId
                };

                // Create a RuntimeError instance for the error context
                var runtimeError = new RuntimeError
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = "Rollback operation"
                };

                // Create an error context for the rollback
                var errorContext = new ErrorContext(runtimeError, "rollback", DateTime.UtcNow) 
                { 
                    ContextId = Guid.NewGuid().ToString()
                };

                // Execute the rollback
                var rollbackResult = await _executor.RollbackActionAsync(actionId, errorContext);

                _logger.LogInformation("Rollback for action {ActionId} completed with result: {IsSuccessful}", 
                    actionId, rollbackResult.IsSuccessful);

                // Update metrics
                metrics.Complete(rollbackResult.IsSuccessful, rollbackResult.ErrorMessage);

                return rollbackResult.IsSuccessful 
                    ? RuntimeErrorSage.Domain.Enums.RollbackStatus.Completed 
                    : RuntimeErrorSage.Domain.Enums.RollbackStatus.Failed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rolling back remediation action {ActionId}: {ErrorMessage}", 
                    actionId, ex.Message);
                
                return RuntimeErrorSage.Domain.Enums.RollbackStatus.Failed;
            }
        }

        /// <summary>
        /// Gets the status of a remediation action.
        /// </summary>
        /// <param name="actionId">The ID of the action to check.</param>
        /// <returns>The action status.</returns>
        public async Task<RemediationResult> GetActionStatusAsync(string actionId)
        {
            _logger.LogInformation("Getting status for remediation action {ActionId}", actionId);

            // Create a RuntimeError instance for the error context
            var runtimeError = new RuntimeError
            {
                Id = Guid.NewGuid().ToString(),
                Message = "Status check"
            };
            
            // Create an error context for the status check
            var errorContext = new ErrorContext(runtimeError, "status_check", DateTime.UtcNow)
            { 
                ContextId = Guid.NewGuid().ToString()
            };

            try
            {
                var result = await _executor.GetActionStatusAsync(actionId, errorContext);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting status for remediation action {ActionId}: {ErrorMessage}", 
                    actionId, ex.Message);
                
                return new RemediationResult(errorContext, RemediationStatusEnum.Failed, $"Status retrieval error: {ex.Message}", ex.StackTrace)
                {
                    IsSuccessful = false,
                    ActionId = actionId,
                    ExecutionId = string.Empty,
                    Error = ex.ToString(),
                    Exception = ex,
                    Timestamp = DateTime.UtcNow
                };
            }
        }
    }
} 


