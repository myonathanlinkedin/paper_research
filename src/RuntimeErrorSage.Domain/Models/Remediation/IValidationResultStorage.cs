using RuntimeErrorSage.Model.Models.Validation;

namespace RuntimeErrorSage.Model.Models.Remediation
{
    public interface IValidationResultStorage
    {
        void StoreResult(string actionId, ValidationResult result);
        ValidationResult GetResult(string actionId);
        void ClearResults();
    }
} 