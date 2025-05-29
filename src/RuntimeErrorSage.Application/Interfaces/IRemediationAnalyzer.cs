using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation;

namespace RuntimeErrorSage.Application.Interfaces
{
    /// <summary>
    /// Interface for analyzing remediation operations.
    /// </summary>
    public interface IRemediationAnalyzer
    {
        /// <summary>
        /// Gets whether the analyzer is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the analyzer name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the analyzer version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Analyzes an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation analysis.</returns>
        Task<RemediationAnalysis> AnalyzeErrorAsync(ErrorContext context);

        /// <summary>
        /// Gets the impact of a remediation operation.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation impact.</returns>
        Task<RemediationImpact> GetImpactAsync(ErrorContext context);

        /// <summary>
        /// Gets the risk assessment for a remediation operation.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The risk assessment.</returns>
        Task<RiskAssessment> GetRiskAssessmentAsync(ErrorContext context);

        /// <summary>
        /// Gets the recommended remediation strategy.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The recommended strategy.</returns>
        Task<RemediationStrategyModel> GetRecommendedStrategyAsync(ErrorContext context);

        /// <summary>
        /// Gets the confidence level for a remediation operation.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The confidence level (0-1).</returns>
        Task<double> GetConfidenceLevelAsync(ErrorContext context);
    }
} 






