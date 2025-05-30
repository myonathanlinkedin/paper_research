using System;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Interfaces;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    public class DefaultValidationStateChecker : IValidationStateChecker
    {
        public async Task<bool> IsValidAsync(ValidationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            // Implementation logic based on context
            return await Task.FromResult(context.IsValid);
        }

        public async Task<ValidationResult> GetValidationResultAsync(ValidationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            // Implementation logic based on context
            return await Task.FromResult(
                new ValidationResult(
                    context,
                    isValid: context.IsValid,
                    severity: context.IsValid ? ValidationSeverity.Info : ValidationSeverity.Error
                )
            );
        }

        public bool IsValidState(IRemediationAction action)
        {
            ArgumentNullException.ThrowIfNull(action);
            return action.Status == RemediationStatusEnum.InProgress;
        }

        public static bool RequiresApproval(IRemediationAction action)
        {
            ArgumentNullException.ThrowIfNull(action);
            return action.RequiresManualApproval && action.Status == RemediationStatusEnum.Waiting;
        }
    }
} 
