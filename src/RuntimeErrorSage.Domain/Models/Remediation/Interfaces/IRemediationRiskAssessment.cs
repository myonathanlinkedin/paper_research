using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Model.Models.Remediation.Interfaces
{
    /// <summary>
    /// Interface for remediation risk assessment functionality.
    /// </summary>
    public interface IRemediationRiskAssessment
    {
        /// <summary>
        /// Calculates the risk level for a remediation action.
        /// </summary>
        /// <param name="action">The remediation action to assess.</param>
        /// <returns>The calculated risk level.</returns>
        RemediationRiskLevel CalculateRiskLevel(RemediationAction action);

        /// <summary>
        /// Generates a list of potential issues based on the remediation action.
        /// </summary>
        /// <param name="action">The remediation action to analyze.</param>
        /// <returns>A list of potential issues.</returns>
        List<string> GeneratePotentialIssues(RemediationAction action);

        /// <summary>
        /// Generates a list of mitigation steps based on the remediation action.
        /// </summary>
        /// <param name="action">The remediation action to analyze.</param>
        /// <returns>A list of mitigation steps.</returns>
        List<string> GenerateMitigationSteps(RemediationAction action);
    }
} 
