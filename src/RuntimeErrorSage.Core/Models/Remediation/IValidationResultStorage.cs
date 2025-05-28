using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    public interface IValidationResultStorage
    {
        void StoreResult(string actionId, ValidationResult result);
        ValidationResult GetResult(string actionId);
        void ClearResults();
    }
} 