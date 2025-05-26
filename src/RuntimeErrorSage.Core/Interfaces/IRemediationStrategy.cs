using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Interface for remediation strategies.
    /// </summary>
    public interface IRemediationStrategy
    {
        /// <summary>
        /// Gets whether the strategy is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the strategy name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the strategy version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the strategy priority.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Gets whether the strategy can be rolled back.
        /// </summary>
        bool CanRollback { get; }

        /// <summary>
        /// Executes the remediation strategy.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation result.</returns>
        Task<RemediationResult> ExecuteAsync(ErrorContext context);

        /// <summary>
        /// Validates if the strategy can be applied.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>True if the strategy can be applied; otherwise, false.</returns>
        Task<bool> CanApplyAsync(ErrorContext context);

        /// <summary>
        /// Gets the rollback plan for this strategy.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The rollback plan.</returns>
        Task<RollbackPlan> GetRollbackPlanAsync(ErrorContext context);
    }
} 