using RuntimeErrorSage.Core.Models.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    public interface IValidationStateChecker
    {
        bool IsValidState(IRemediationAction action);
    }
} 