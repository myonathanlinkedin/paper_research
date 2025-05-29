using RuntimeErrorSage.Model.Models.Remediation.Interfaces;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Model.Models.Remediation
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