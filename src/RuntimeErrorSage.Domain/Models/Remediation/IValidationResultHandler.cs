using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    public interface IValidationResultHandler
    {
        void HandleResult(ValidationResult result);
    }
} 