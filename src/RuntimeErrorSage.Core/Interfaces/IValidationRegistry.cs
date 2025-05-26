using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Interfaces
{
    public interface IValidationRegistry
    {
        Task<ValidationResult> ValidateAsync(string validationId, object context);
        Task<bool> RegisterValidatorAsync(string validationType, IValidator validator);
        Task<bool> UnregisterValidatorAsync(string validationType);
        Task<IValidator> GetValidatorAsync(string validationType);
    }

    public interface IValidator
    {
        Task<ValidationResult> ValidateAsync(object context);
        bool CanValidate(object context);
    }
} 