using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Contracts.Interfaces
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
        /// Gets or sets the priority of the action.
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        /// Gets or sets the impact level of the action.
        /// </summary>
        int Impact { get; set; }

        /// <summary>
        /// Gets or sets the risk level of the action.
        /// </summary>
        int RiskLevel { get; set; }

        /// <summary>
        /// Gets or sets the status of the action.
        /// </summary>
        int Status { get; set; }

        /// <summary>
        /// Gets or sets whether the action can be rolled back.
        /// </summary>
        bool CanRollback { get; set; }

        /// <summary>
        /// Gets the parameters for the action.
        /// </summary>
        IReadOnlyDictionary<string, object> Parameters { get; }

        /// <summary>
        /// Gets the validation rules.
        /// </summary>
        IReadOnlyList<string> ValidationRules { get; }

        /// <summary>
        /// Gets or sets the error type this action addresses.
        /// </summary>
        string ErrorType { get; set; }

        /// <summary>
        /// Gets or sets the scope of impact for this remediation action.
        /// </summary>
        int ImpactScope { get; set; }
    }
} 