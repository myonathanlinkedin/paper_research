using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    public class DefaultValidationStateChecker : IValidationStateChecker
    {
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