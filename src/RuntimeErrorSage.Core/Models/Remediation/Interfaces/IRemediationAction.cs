using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Remediation;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.Remediation.Interfaces
{
    /// <summary>
    /// Interface for remediation actions.
    /// </summary>
    public interface IRemediationAction
    {
        /// <summary>
        /// Gets or sets the action type.
        /// </summary>
        string ActionType { get; set; }

        /// <summary>
        /// Gets or sets the action name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the action description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the action parameters.
        /// </summary>
        Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Validates the action.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateAsync(ErrorContext context);

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation result.</returns>
        Task<RemediationResult> ExecuteAsync(ErrorContext context);
    }
} 

