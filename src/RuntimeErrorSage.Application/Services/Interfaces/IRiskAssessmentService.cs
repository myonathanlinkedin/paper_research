using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Interfaces;

namespace RuntimeErrorSage.Application.Services.Interfaces;

/// <summary>
/// Interface for assessing risks associated with remediation actions.
/// </summary>
public interface IRiskAssessmentService
{
    /// <summary>
    /// Assesses the risk of a remediation action.
    /// </summary>
    /// <param name="action">The remediation action to assess.</param>
    /// <returns>A risk assessment for the action.</returns>
    Task<RiskAssessment> AssessRiskAsync(RemediationAction action);

    /// <summary>
    /// Gets a risk assessment by its ID.
    /// </summary>
    /// <param name="id">The assessment ID.</param>
    /// <returns>The risk assessment.</returns>
    Task<RiskAssessment> GetAssessmentAsync(string id);

    /// <summary>
    /// Gets risk assessments by correlation ID.
    /// </summary>
    /// <param name="correlationId">The correlation ID.</param>
    /// <returns>The risk assessments.</returns>
    Task<IEnumerable<RiskAssessment>> GetAssessmentsByCorrelationIdAsync(string correlationId);

    /// <summary>
    /// Updates a risk assessment.
    /// </summary>
    /// <param name="assessment">The assessment to update.</param>
    /// <returns>The updated assessment.</returns>
    Task<RiskAssessment> UpdateAssessmentAsync(RiskAssessment assessment);

    /// <summary>
    /// Deletes a risk assessment.
    /// </summary>
    /// <param name="id">The assessment ID.</param>
    /// <returns>True if the assessment was deleted, false otherwise.</returns>
    Task<bool> DeleteAssessmentAsync(string id);
} 
