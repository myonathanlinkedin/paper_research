using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Services.Interfaces;

/// <summary>
/// Service for selecting appropriate remediation strategies.
/// </summary>
public interface IRemediationStrategySelector
{
    /// <summary>
    /// Selects the most appropriate remediation strategy for an error analysis result.
    /// </summary>
    /// <param name="analysisResult">The error analysis result</param>
    /// <returns>The selected remediation strategy, or null if no suitable strategy is found</returns>
    Task<IRemediationStrategy?> SelectStrategyAsync(ErrorAnalysisResult analysisResult);

    /// <summary>
    /// Gets all applicable strategies for an error analysis result.
    /// </summary>
    /// <param name="analysisResult">The error analysis result</param>
    /// <returns>A collection of applicable strategies ordered by priority</returns>
    Task<IEnumerable<IRemediationStrategy>> GetApplicableStrategiesAsync(ErrorAnalysisResult analysisResult);

    /// <summary>
    /// Validates if a strategy is applicable to an error analysis result.
    /// </summary>
    /// <param name="strategy">The strategy to validate</param>
    /// <param name="analysisResult">The error analysis result</param>
    /// <returns>True if the strategy is applicable, false otherwise</returns>
    Task<bool> IsStrategyApplicableAsync(IRemediationStrategy strategy, ErrorAnalysisResult analysisResult);
} 