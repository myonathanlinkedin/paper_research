using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation;

namespace RuntimeErrorSage.Application.Interfaces
{
    /// <summary>
    /// Interface for validating remediation actions.
    /// </summary>
    public interface IRemediationActionValidator
    {
        /// <summary>
        /// Validates a remediation suggestion against an error context.
        /// </summary>
        /// <param name="suggestion">The remediation suggestion.</param>
        /// <param name="context">The error context.</param>
        /// <returns>True if the action is valid; otherwise, false.</returns>
        Task<bool> ValidateAsync(RemediationSuggestion suggestion, ErrorContext context);
    }
} 


