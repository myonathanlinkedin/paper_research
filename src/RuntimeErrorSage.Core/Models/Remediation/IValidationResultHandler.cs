using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    public interface IValidationResultHandler
    {
        void HandleResult(ValidationResult result);
    }
} 