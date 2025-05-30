using RuntimeErrorSage.Domain.Models.Validation;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    public interface IValidationResultStorage
    {
        void StoreResult(string actionId, ValidationResult result);
        ValidationResult GetResult(string actionId);
        void ClearResults();
    }
} 
