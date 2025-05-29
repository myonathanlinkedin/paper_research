using RuntimeErrorSage.Application.Models.Remediation.Interfaces;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    public interface IValidationStateChecker
    {
        bool IsValidState(IRemediationAction action);
    }
} 