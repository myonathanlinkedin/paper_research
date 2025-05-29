using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    public class DefaultValidationStateChecker : IValidationStateChecker
    {
        public bool IsValidState(IRemediationAction action)
        {
            if (action.Status == RemediationStatusEnum.Running)
            {
                return false;
            }

            if (action.RequiresManualApproval && action.Status != RemediationStatusEnum.WaitingForApproval)
            {
                return false;
            }

            return true;
        }
    }
} 