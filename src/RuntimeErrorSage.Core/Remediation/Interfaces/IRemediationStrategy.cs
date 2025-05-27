using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    /// <summary>
    /// Interface for remediation strategies.
    /// </summary>
    public interface IRemediationStrategy
    {
        /// <summary>
        /// Gets or sets the strategy name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the strategy description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the strategy parameters.
        /// </summary>
        Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the error types this strategy can handle.
        /// </summary>
        ISet<string> SupportedErrorTypes { get; }

        /// <summary>
        /// Executes the remediation strategy.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation result.</returns>
        Task<RemediationResult> ExecuteAsync(ErrorContext context);

        /// <summary>
        /// Validates if this strategy can handle the given error.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>True if the strategy can handle the error; otherwise, false.</returns>
        Task<bool> CanHandleErrorAsync(ErrorContext context);

        /// <summary>
        /// Validates the strategy for a given error context.
        /// </summary>
        /// <param name="errorContext">The error context to validate against.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateAsync(ErrorContext errorContext);

        /// <summary>
        /// Gets the priority of this strategy.
        /// </summary>
        /// <param name="errorContext">The error context.</param>
        /// <returns>The remediation priority.</returns>
        Task<RemediationPriority> GetPriorityAsync(ErrorContext errorContext);
    }
} 