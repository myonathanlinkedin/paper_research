using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Interfaces
{
    /// <summary>
    /// Interface for remediation actions.
    /// </summary>
    public interface IRemediationAction
    {
        /// <summary>
        /// Gets the unique identifier for the action.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the unique identifier for the action (alias for Id).
        /// </summary>
        string ActionId => Id;

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type of the action.
        /// </summary>
        string ActionType { get; }

        /// <summary>
        /// Gets the context for the action.
        /// </summary>
        ErrorContext Context { get; }

        /// <summary>
        /// Gets or sets the action description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the priority of the action.
        /// </summary>
        RemediationPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the impact level of the action.
        /// </summary>
        ImpactLevel Impact { get; set; }

        /// <summary>
        /// Gets or sets the risk level of the action.
        /// </summary>
        RiskLevel RiskLevel { get; set; }

        /// <summary>
        /// Gets or sets the status of the action.
        /// </summary>
        RemediationStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the action requires manual approval.
        /// </summary>
        bool RequiresManualApproval { get; set; }

        /// <summary>
        /// Gets or sets whether the action can be rolled back.
        /// </summary>
        bool CanRollback { get; set; }

        /// <summary>
        /// Gets or sets the rollback action.
        /// </summary>
        IRemediationAction RollbackAction { get; set; }

        /// <summary>
        /// Gets or sets the parameters for the action.
        /// </summary>
        Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the validation rules.
        /// </summary>
        List<string> ValidationRules { get; set; }

        /// <summary>
        /// Gets or sets the error type this action addresses.
        /// </summary>
        string ErrorType { get; set; }

        /// <summary>
        /// Executes the remediation action.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExecuteAsync();

        /// <summary>
        /// Validates the remediation action.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<ValidationResult> ValidateAsync();

        /// <summary>
        /// Rolls back the remediation action.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<Domain.Enums.RollbackStatusEnum> RollbackAsync();

        /// <summary>
        /// Gets the estimated impact of this action.
        /// </summary>
        /// <returns>The estimated impact.</returns>
        Task<RemediationImpact> GetEstimatedImpactAsync();
    }
} 