using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.Remediation.Interfaces
{
    /// <summary>
    /// Interface for remediation strategies that can be executed to resolve errors.
    /// </summary>
    public interface IRemediationStrategy
    {
        /// <summary>
        /// Gets or sets the unique identifier of the strategy.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the strategy.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the priority of the strategy.
        /// </summary>
        RemediationPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the description of the strategy.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the parameters for the strategy.
        /// </summary>
        Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets the collection of error types that this strategy can remediate.
        /// </summary>
        ISet<string> SupportedErrorTypes { get; }

        /// <summary>
        /// Executes the remediation strategy for the given error context.
        /// </summary>
        /// <param name="context">The error context to remediate.</param>
        /// <returns>The remediation result.</returns>
        Task<RemediationResult> ExecuteAsync(ErrorContext context);

        /// <summary>
        /// Validates whether the strategy can be applied to the given error context.
        /// </summary>
        /// <param name="context">The error context to validate.</param>
        /// <returns>True if the strategy can be applied, false otherwise.</returns>
        Task<bool> CanApplyAsync(ErrorContext context);

        /// <summary>
        /// Gets the estimated impact of applying this strategy.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The impact assessment.</returns>
        Task<RemediationImpact> GetImpactAsync(ErrorContext context);

        /// <summary>
        /// Gets the risk assessment for applying this strategy.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The risk assessment.</returns>
        Task<RiskAssessment> GetRiskAsync(ErrorContext context);

        /// <summary>
        /// Validates the strategy's configuration.
        /// </summary>
        /// <returns>True if the configuration is valid, false otherwise.</returns>
        Task<bool> ValidateConfigurationAsync();

        /// <summary>
        /// Validates if the strategy can be applied to the given error context.
        /// </summary>
        /// <param name="context">The error context to validate.</param>
        /// <returns>A task representing the asynchronous operation that returns a validation result.</returns>
        Task<ValidationResult> ValidateAsync(ErrorContext context);
    }
} 
