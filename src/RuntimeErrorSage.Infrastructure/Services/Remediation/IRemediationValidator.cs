using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Services.Remediation
{
    /// <summary>
    /// Interface for validating remediation actions.
    /// </summary>
    public interface IRemediationValidator
    {
        /// <summary>
        /// Validates a remediation action.
        /// </summary>
        /// <param name="action">The action to validate.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateActionAsync(RemediationAction action);
    }
} 
