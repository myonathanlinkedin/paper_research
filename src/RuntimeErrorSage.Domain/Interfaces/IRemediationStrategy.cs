using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Interfaces
{
    /// <summary>
    /// Interface for remediation strategies.
    /// </summary>
    public interface IRemediationStrategy
    {
        /// <summary>
        /// Gets the unique identifier for the strategy.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets or sets the name of the strategy.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the strategy.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets the version of the strategy.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets a value indicating whether the strategy is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets or sets the priority of the strategy.
        /// </summary>
        RemediationPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the priority value as integer for interoperability.
        /// </summary>
        int? PriorityValue { get; set; }

        /// <summary>
        /// Gets or sets the risk level of the strategy.
        /// </summary>
        RiskLevel RiskLevel { get; set; }

        /// <summary>
        /// Gets or sets the parameters of the strategy.
        /// </summary>
        Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets the supported error types.
        /// </summary>
        ISet<string> SupportedErrorTypes { get; }

        /// <summary>
        /// Gets the list of remediation actions.
        /// </summary>
        List<RemediationAction> Actions { get; }

        /// <summary>
        /// Gets the creation timestamp.
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// Gets or sets the status of the strategy.
        /// </summary>
        RemediationStatusEnum Status { get; set; }

        /// <summary>
        /// Gets the priority of the strategy asynchronously.
        /// </summary>
        /// <returns>The priority of the strategy.</returns>
        Task<RemediationPriority> GetPriorityAsync();

        /// <summary>
        /// Determines whether the strategy applies to the specified error context.
        /// </summary>
        /// <param name="errorContext">The error context.</param>
        /// <returns>True if the strategy applies; otherwise, false.</returns>
        bool AppliesTo(ErrorContext errorContext);

        /// <summary>
        /// Creates remediation actions for the specified error context.
        /// </summary>
        /// <param name="errorContext">The error context.</param>
        /// <returns>A list of remediation actions.</returns>
        Task<IEnumerable<RemediationAction>> CreateActionsAsync(ErrorContext errorContext);
    }
} 