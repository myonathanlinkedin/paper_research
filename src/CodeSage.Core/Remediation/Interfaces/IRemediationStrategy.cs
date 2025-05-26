using System.Threading.Tasks;
using CodeSage.Core.Models;
using CodeSage.Core.Remediation.Models.Execution;
using CodeSage.Core.Models.Error;
using CodeSage.Core.Remediation.Models.Common;

namespace CodeSage.Core.Remediation.Interfaces
{
    /// <summary>
    /// Defines the interface for custom remediation strategies.
    /// </summary>
    public interface IRemediationStrategy
    {
        /// <summary>
        /// Gets the name of the remediation strategy.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the remediation strategy.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the priority of the strategy (1-5, where 5 is highest).
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Determines if this strategy can handle the given analysis result.
        /// </summary>
        /// <param name="analysisResult">The analysis result to evaluate</param>
        /// <returns>True if the strategy can handle the result, false otherwise</returns>
        bool CanHandle(ErrorAnalysisResult analysisResult);

        /// <summary>
        /// Applies the remediation strategy.
        /// </summary>
        /// <param name="analysisResult">The analysis result to act upon</param>
        /// <returns>The result of the remediation attempt</returns>
        Task<RemediationExecution> ApplyAsync(ErrorAnalysisResult analysisResult);

        /// <summary>
        /// Validates if the strategy can be applied to the given context.
        /// </summary>
        /// <param name="analysisResult">The analysis result to validate</param>
        /// <returns>True if the strategy is valid for the context, false otherwise</returns>
        Task<bool> ValidateAsync(ErrorAnalysisResult analysisResult);

        /// <summary>
        /// Gets the estimated impact of applying this strategy.
        /// </summary>
        /// <param name="analysisResult">The analysis result to evaluate</param>
        /// <returns>The estimated impact of the strategy</returns>
        Task<RemediationImpact> GetEstimatedImpactAsync(ErrorAnalysisResult analysisResult);
    }
} 