using RuntimeErrorSage.Model.Models.Validation;

namespace RuntimeErrorSage.Model.Models.Remediation
{
    public interface IValidationResultHandler
    {
        void HandleResult(ValidationResult result);
    }
} 