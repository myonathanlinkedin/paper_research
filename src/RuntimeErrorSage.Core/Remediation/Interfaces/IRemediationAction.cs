using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    /// <summary>
    /// Represents a remediation action that can be executed to resolve an error.
    /// </summary>
    public interface IRemediationAction
    {
        /// <summary>
        /// Gets the unique identifier for this action.
        /// </summary>
        string ActionId { get; }

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the action.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the priority of the action.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Gets or sets parameters for the action.
        /// </summary>
        Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets the error context this action is associated with.
        /// </summary>
        ErrorContext Context { get; }

        /// <summary>
        /// Executes the remediation action.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a result indicating success or failure.</returns>
        Task<RemediationActionResult> ExecuteAsync();

        /// <summary>
        /// Rolls back the remediation action.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a result indicating success or failure.</returns>
        Task<RemediationActionResult> RollbackAsync();

        /// <summary>
        /// Validates the remediation action.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a validation result.</returns>
        Task<Models.Validation.ValidationResult> ValidateAsync();

        /// <summary>
        /// Gets the estimated impact of this action.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with the impact result.</returns>
        Task<RemediationImpact> GetEstimatedImpactAsync();
    }
} 