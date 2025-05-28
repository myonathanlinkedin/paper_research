using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    /// <summary>
    /// Interface for remediation risk assessment operations.
    /// </summary>
    public interface IRemediationRiskAssessment
    {
        /// <summary>
        /// Assesses the risk of a remediation action.
        /// </summary>
        /// <param name="action">The remediation action to assess.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The risk assessment result.</returns>
        Task<RiskAssessment> AssessRiskAsync(RemediationAction action, ErrorContext context);

        /// <summary>
        /// Assesses the risk of a remediation strategy.
        /// </summary>
        /// <param name="strategy">The remediation strategy to assess.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The risk assessment result.</returns>
        Task<RiskAssessment> AssessStrategyRiskAsync(IRemediationStrategy strategy, ErrorContext context);

        /// <summary>
        /// Assesses the risk of a remediation plan.
        /// </summary>
        /// <param name="plan">The remediation plan to assess.</param>
        /// <returns>The risk assessment result.</returns>
        Task<RiskAssessment> AssessPlanRiskAsync(RemediationPlan plan);

        /// <summary>
        /// Gets the risk level for a given error type.
        /// </summary>
        /// <param name="errorType">The error type to assess.</param>
        /// <returns>The risk level.</returns>
        Task<RemediationRiskLevel> GetRiskLevelForErrorTypeAsync(string errorType);

        /// <summary>
        /// Gets the risk metrics for a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation ID.</param>
        /// <returns>The risk metrics.</returns>
        Task<RiskMetrics> GetRiskMetricsAsync(string remediationId);
    }
} 