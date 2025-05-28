using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for risk assessment services.
    /// </summary>
    public interface IRiskAssessmentService
    {
        /// <summary>
        /// Assesses the risk of a remediation action.
        /// </summary>
        /// <param name="action">The remediation action to assess.</param>
        /// <returns>A risk assessment for the action.</returns>
        Task<RiskAssessment> AssessRiskAsync(RemediationAction action);
    }
} 
