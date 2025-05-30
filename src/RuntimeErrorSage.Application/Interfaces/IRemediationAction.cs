using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Interfaces
{
    /// <summary>
    /// Interface for remediation actions.
    /// </summary>
    public interface IRemediationAction
    {
        /// <summary>
        /// Gets or sets the unique identifier for this action.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the action name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the action description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the action type.
        /// </summary>
        string ActionType { get; set; }

        /// <summary>
        /// Gets or sets the error context.
        /// </summary>
        ErrorContext Context { get; set; }

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
        /// Gets or sets the scope of impact for this remediation action.
        /// </summary>
        RemediationActionImpactScope ImpactScope { get; set; }

        /// <summary>
        /// Executes the remediation action.
        /// </summary>
        /// <returns>The result of the action execution.</returns>
        Task<RemediationResult> ExecuteAsync();

        /// <summary>
        /// Validates the remediation action.
        /// </summary>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateAsync();

        /// <summary>
        /// Rolls back the remediation action.
        /// </summary>
        /// <returns>The rollback status.</returns>
        Task<RuntimeErrorSage.Domain.Models.Remediation.RollbackStatus> RollbackAsync();

        /// <summary>
        /// Gets the estimated impact of this action.
        /// </summary>
        /// <returns>The estimated impact.</returns>
        Task<RemediationImpact> GetEstimatedImpactAsync();
    }
} 
