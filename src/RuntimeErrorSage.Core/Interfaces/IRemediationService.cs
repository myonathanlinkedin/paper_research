using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Metrics;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Interface for managing remediation operations.
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
        /// Creates a remediation plan.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation plan.</returns>
        Task<RemediationPlan> CreatePlanAsync(ErrorContext context);

        /// <summary>
        /// Validates a remediation plan.
        /// </summary>
        /// <param name="plan">The remediation plan.</param>
        /// <returns>True if the plan is valid, false otherwise.</returns>
        Task<bool> ValidatePlanAsync(RemediationPlan plan);

        /// <summary>
        /// Gets the status of a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation ID.</param>
        /// <returns>The remediation status.</returns>
        Task<RemediationStatus> GetStatusAsync(string remediationId);

        /// <summary>
        /// Gets metrics for a remediation operation.
        /// </summary>
        /// <param name="remediationId">The remediation ID.</param>
        /// <returns>The remediation metrics.</returns>
        Task<RemediationMetrics> GetMetricsAsync(string remediationId);
    }
} 