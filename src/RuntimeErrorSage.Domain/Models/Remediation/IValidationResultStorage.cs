using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    public interface IValidationResultStorage
    {
        void StoreResult(string actionId, ValidationResult result);
        ValidationResult GetResult(string actionId);
        void ClearResults();
    }
} 