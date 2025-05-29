using RuntimeErrorSage.Model.Models.Remediation.Interfaces;

namespace RuntimeErrorSage.Model.Models.Remediation
{
    public interface IValidationStateChecker
    {
        bool IsValidState(IRemediationAction action);
    }
} 