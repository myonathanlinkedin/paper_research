using System.Threading.Tasks;
using RuntimeErrorSage.Model.Models.Remediation;

namespace RuntimeErrorSage.Model.Services.Interfaces
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
