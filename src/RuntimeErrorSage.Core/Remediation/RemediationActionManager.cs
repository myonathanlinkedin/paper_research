using System.Collections.ObjectModel;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Validation;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Remediation
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
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(validator);
            ArgumentNullException.ThrowIfNull(executor);

            _logger = logger;
            _validator = validator;
            _executor = executor;
        }

        public async Task<RemediationResult> ExecuteActionAsync(RemediationAction action)
        {
            ArgumentNullException.ThrowIfNull(action);

            try
            {
                return await _executor.ExecuteActionAsync(action, new ErrorContext());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing action {ActionName}", action.Name);
                return new RemediationResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<ValidationResult> ValidateActionAsync(RemediationAction action)
        {
            ArgumentNullException.ThrowIfNull(action);

            try
            {
                return await _validator.ValidateActionAsync(action);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating action {ActionName}", action.Name);
                return new ValidationResult { IsValid = false, Errors = new Collection<string> { ex.Message } };
            }
        }

        public async Task<RollbackStatus> RollbackActionAsync(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);

            try
            {
                var status = new RollbackStatus
                {
                    ActionId = actionId,
                    Status = RollbackState.Completed,
                    Message = "Action rolled back successfully"
                };
                return status;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rolling back action {ActionId}", actionId);
                return new RollbackStatus { Status = RollbackState.Failed, Message = ex.Message };
            }
        }

        public async Task<RemediationResult> GetActionStatusAsync(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);

            try
            {
                return await _executor.GetActionStatusAsync(actionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting action status for {ActionId}", actionId);
                return new RemediationResult { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
} 








