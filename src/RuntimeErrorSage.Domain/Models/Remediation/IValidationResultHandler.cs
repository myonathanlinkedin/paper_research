using RuntimeErrorSage.Domain.Models.Validation;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    public interface IValidationResultHandler
    {
        void HandleResult(ValidationResult result);
    }
} 
