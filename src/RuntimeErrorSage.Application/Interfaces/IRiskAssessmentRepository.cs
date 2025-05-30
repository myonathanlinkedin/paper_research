using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models;
using RuntimeErrorSage.Domain.Models.Remediation;

namespace RuntimeErrorSage.Application.Interfaces;

/// <summary>
/// Repository interface for risk assessment data.
/// </summary>
public interface IRiskAssessmentRepository
{
    /// <summary>
    /// Gets a risk assessment by its ID.
    /// </summary>
    /// <param name="id">The risk assessment ID.</param>
    /// <returns>The risk assessment if found, null otherwise.</returns>
    Task<RiskAssessment> GetByIdAsync(string id);

    /// <summary>
    /// Gets all risk assessments for a given correlation ID.
    /// </summary>
    /// <param name="correlationId">The correlation ID.</param>
    /// <returns>A list of risk assessments.</returns>
    Task<IEnumerable<RiskAssessment>> GetByCorrelationIdAsync(string correlationId);

    /// <summary>
    /// Saves a risk assessment.
    /// </summary>
    /// <param name="assessment">The risk assessment to save.</param>
    /// <returns>The saved risk assessment.</returns>
    Task<RiskAssessment> SaveAsync(RiskAssessment assessment);

    /// <summary>
    /// Updates a risk assessment.
    /// </summary>
    /// <param name="assessment">The risk assessment to update.</param>
    /// <returns>The updated risk assessment.</returns>
    Task<RiskAssessment> UpdateAsync(RiskAssessment assessment);

    /// <summary>
    /// Deletes a risk assessment.
    /// </summary>
    /// <param name="id">The ID of the risk assessment to delete.</param>
    /// <returns>True if the deletion was successful, false otherwise.</returns>
    Task<bool> DeleteAsync(string id);
} 
