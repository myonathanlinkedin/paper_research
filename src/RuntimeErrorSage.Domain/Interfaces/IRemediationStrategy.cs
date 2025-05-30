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
        /// Gets the name of the strategy.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the strategy.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a value indicating whether the strategy is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets or sets the risk level of the strategy.
        /// </summary>
        RiskLevel RiskLevel { get; set; }

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