using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for remediation services.
    /// </summary>
    public interface IRemediationService
    {
        /// <summary>
        /// Gets whether the service is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the service name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the service version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Applies remediation to an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation result.</returns>
        Task<RemediationResult> ApplyRemediationAsync(ErrorContext context);

        /// <summary>
        /// Creates a remediation plan for an error context.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation plan.</returns>
        Task<RemediationPlan> CreatePlanAsync(ErrorContext context);

        /// <summary>
        /// Validates a remediation plan.
        /// </summary>
        /// <param name="plan">The remediation plan.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidatePlanAsync(RemediationPlan plan);

        /// <summary>
        /// Gets metrics for a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation identifier.</param>
        /// <returns>The remediation metrics.</returns>
        Task<Dictionary<string, double>> GetMetricsAsync(string remediationId);

        /// <summary>
        /// Gets the status of a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation identifier.</param>
        /// <returns>The remediation status.</returns>
        Task<RemediationStatus> GetStatusAsync(string remediationId);
    }
} 