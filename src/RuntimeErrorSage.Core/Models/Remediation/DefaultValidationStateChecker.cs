using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Remediation
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