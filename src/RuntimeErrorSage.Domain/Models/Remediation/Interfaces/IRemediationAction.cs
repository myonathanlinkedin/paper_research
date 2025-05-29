using System;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Models.Remediation;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Models.Remediation.Interfaces
{
    /// <summary>
    /// Interface for remediation actions.
    /// </summary>
    public interface IRemediationAction
    {
        /// <summary>
        /// Gets or sets the unique identifier for this action.
        /// </summary>
        string ActionId { get; set; }

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
        /// Gets or sets the priority of the action.
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        /// Gets or sets the action parameters.
        /// </summary>
        Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the error context this action is associated with.
        /// </summary>
        ErrorContext Context { get; set; }

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

        /// <summary>
        /// Rolls back the remediation action.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a result indicating success or failure.</returns>
        Task<RemediationResult> RollbackAsync();

        /// <summary>
        /// Gets the estimated impact of this action.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with the impact result.</returns>
        Task<RemediationImpact> GetEstimatedImpactAsync();
    }
} 

