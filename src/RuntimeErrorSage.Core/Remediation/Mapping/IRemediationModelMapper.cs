using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RemediationActionExecution = RuntimeErrorSage.Domain.Models.Execution.RemediationActionExecution;

namespace RuntimeErrorSage.Core.Remediation.Mapping
{
    /// <summary>
    /// Maps between different remediation model types.
    /// Provides conversion between domain models to maintain consistency.
    /// </summary>
    public interface IRemediationModelMapper
    {
        /// <summary>
        /// Converts a RemediationSuggestion to a RemediationAction.
        /// </summary>
        RemediationAction ToAction(RemediationSuggestion suggestion, ErrorContext context, IRemediationStrategyMapper strategyMapper);

        /// <summary>
        /// Converts a RemediationActionExecution to a RemediationResult.
        /// </summary>
        RemediationResult ToResult(RemediationActionExecution execution, ErrorContext context);

        /// <summary>
        /// Creates a RemediationPlan from analysis and actions.
        /// </summary>
        RemediationPlan ToPlan(string planId, string name, List<RemediationAction> actions, ErrorContext context);
    }
}

