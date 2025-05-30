using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Remediation;

namespace RuntimeErrorSage.Application.Interfaces
{
    /// <summary>
    /// Interface for assessing risks associated with remediation strategies.
    /// </summary>
    public interface IRemediationRiskAssessment
    {
        /// <summary>
        /// Assesses the risk of a remediation strategy.
        /// </summary>
        /// <param name="strategy">The remediation strategy to assess.</param>
        /// <returns>A risk assessment result.</returns>
        RiskAssessmentModel AssessRisk(RemediationStrategyModel strategy);

        /// <summary>
        /// Gets the risk factors for a remediation strategy.
        /// </summary>
        /// <param name="strategy">The remediation strategy to assess.</param>
        /// <returns>A list of risk factors.</returns>
        List<RiskFactor> GetRiskFactors(RemediationStrategyModel strategy);

        /// <summary>
        /// Gets the risk metrics for a remediation strategy.
        /// </summary>
        /// <param name="strategy">The remediation strategy to assess.</param>
        /// <returns>The risk metrics.</returns>
        RiskMetrics GetRiskMetrics(RemediationStrategyModel strategy);

        /// <summary>
        /// Assesses the risk of a remediation action.
        /// </summary>
        /// <param name="action">The remediation action to assess.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The risk assessment result.</returns>
        Task<RiskAssessmentModel> AssessRiskAsync(RemediationAction action, ErrorContext context);

        /// <summary>
        /// Assesses the risk of a remediation strategy.
        /// </summary>
        /// <param name="strategy">The remediation strategy to assess.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The risk assessment result.</returns>
        Task<RiskAssessmentModel> AssessStrategyRiskAsync(IRemediationStrategy strategy, ErrorContext context);

        /// <summary>
        /// Assesses the risk of a remediation plan.
        /// </summary>
        /// <param name="plan">The remediation plan to assess.</param>
        /// <returns>The risk assessment result.</returns>
        Task<RiskAssessmentModel> AssessPlanRiskAsync(RemediationPlan plan);

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
